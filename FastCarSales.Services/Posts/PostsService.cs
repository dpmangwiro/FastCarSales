namespace FastCarSales.Services.Posts
{
	using System;
	using System.Linq;
	using System.Globalization;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using Cars;
	using Cars.Models;
	using Data;
	using Data.Models;
	using Images;
	using Images.Models;
	using Models;
	using System.Runtime.CompilerServices;

	public class PostsService : IPostsService
	{
		private readonly FastCarSalesDbContext data;
		private readonly ICarsService carsService;
		private readonly IImagesService imagesService;

		public PostsService(FastCarSalesDbContext data, ICarsService carsService, IImagesService imagesService)
		{
			this.data = data;
			this.carsService = carsService;
			this.imagesService = imagesService;
		}

		public async Task<int> CreateAsync(PostFormInputModelDTO inputPost, Car car, string userId, bool isPublic)
		{
			var post = new Post
			{
				Car = car,
				CreatorId = userId,
				PublishedOn = DateTime.UtcNow,
				SellerName = inputPost.SellerName,
				SellerPhoneNumber = inputPost.SellerPhoneNumber,
				SellerEmail = inputPost.SellerEmail,
				IsPublic = isPublic,
			};

			await this.data.Posts.AddAsync(post);
			await this.data.SaveChangesAsync();

			return post.Id;
		}

		public IEnumerable<T> GetPostsByPage<T>(IEnumerable<T> posts, int page, int postsPerPage)
		{
			return posts.Skip((page - 1) * postsPerPage).Take(postsPerPage).ToList();
		}

		public IEnumerable<PostByUserDTO> GetPostsByUser(string userId, int sortingNumber)
		{
			var postsQuery = this.data.Posts
				.Where(p => p.CreatorId == userId && !p.IsDeleted).AsQueryable();

			postsQuery = GetSortedPosts(postsQuery, sortingNumber);

			var posts = postsQuery
				.Select(p => new PostByUserDTO()
				{
					Car = new CarByUserDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Price = p.Car.Price,
						Year = p.Car.Year,
						CoverImage = this.imagesService.GetCoverImagePath(p.Car.Images.ToList()),
					},
					PublishedOn = GetFormattedDate(p.PublishedOn),
					PostID = p.Id
				}).ToList();

			return posts;
		}

		public IEnumerable<PostInListDTO> GetMatchingPosts(SearchPostDTO searchInputModel, int sortingNumber)
		{
			var postsQuery = this.data.Posts.Where(p => !p.IsDeleted && p.IsPublic).AsQueryable();

			if (searchInputModel.Car != null)
			{
				var searchedCarDetails = searchInputModel.Car;

				if (!string.IsNullOrWhiteSpace(searchedCarDetails.TextSearchTerm))
				{
					postsQuery = postsQuery.Where(p =>
						(p.Car.Make.Name + " " + p.Car.CarModel.Name + " " + p.Car.Description).ToLower()
						.Contains(searchedCarDetails.TextSearchTerm.ToLower()));
				}

				if (searchedCarDetails.FromYear is > 0)
				{
					postsQuery = postsQuery.Where(p =>
						p.Car.Year >= searchedCarDetails.FromYear);
				}

				if (searchedCarDetails.ToYear is > 0)
				{
					postsQuery = postsQuery.Where(p =>
						p.Car.Year <= searchedCarDetails.ToYear);
				}

				if (searchedCarDetails.MinEngineCapacity is > 0)
				{
					postsQuery = postsQuery.Where(p =>
						p.Car.EngineCapacity >= searchedCarDetails.MinEngineCapacity);
				}

				if (searchedCarDetails.MaxEngineCapacity is > 0)
				{
					postsQuery = postsQuery.Where(p =>
						p.Car.EngineCapacity <= searchedCarDetails.MaxEngineCapacity);
				}

				if (searchedCarDetails.MinPrice is > 0)
				{
					postsQuery = postsQuery.Where(p =>
						p.Car.Price >= searchedCarDetails.MinPrice);
				}

				if (searchedCarDetails.MaxPrice is > 0)
				{
					postsQuery = postsQuery.Where(p =>
						p.Car.Price <= searchedCarDetails.MaxPrice);
				}
			}

			if (searchInputModel.SelectedBodyTypeIDs.Any())
			{
				postsQuery = postsQuery.Where(p => searchInputModel.SelectedBodyTypeIDs.Contains(p.Car.BodyTypeId));
			}

			if (searchInputModel.SelectedFuelTypesIds.Any())
			{
				postsQuery = postsQuery.Where(p => searchInputModel.SelectedFuelTypesIds.Contains(p.Car.FuelTypeId));
			}

			if (searchInputModel.SelectedTransmissionTypesIds.Any())
			{
				postsQuery = postsQuery.Where(p => searchInputModel.SelectedTransmissionTypesIds.Contains(p.Car.TransmissionTypeId));
			}

			if (searchInputModel.SelectedExtrasIds.Any())
			{
				var searchedExtrasIds = searchInputModel.SelectedExtrasIds;
				var currentQueuedCars = postsQuery.Select(p => p.Car).ToList();
				var allMatchedCarIds = new List<int>();


				foreach (var car in currentQueuedCars)
				{
					var currentCarExtrasIds = data.CarExtras
															.Where(ce => ce.Car.Id == car.Id)
															.Select(ce => ce.ExtraId)
															.ToList();

					//The below code checks if all the searched extras are contained in the current car extras
					if (searchedExtrasIds.Intersect(currentCarExtrasIds).Count() == searchedExtrasIds.Count())
					{
						allMatchedCarIds.Add(car.Id);
					}
				}

				postsQuery = postsQuery.Where(p => allMatchedCarIds.Contains(p.Car.Id));
			}

			if (!postsQuery.Any())
			{
				throw new Exception("Unfortunately, there are no cars in our system that match this search criteria.");
			}

			postsQuery = GetSortedPosts(postsQuery, sortingNumber);

			var posts = postsQuery
				.Select(p => new PostInListDTO()
				{
					Car = new CarInListDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Description = p.Car.Description.Length <= 100 ?
							p.Car.Description
							:
							p.Car.Description.Substring(0, 100) + "...",
						Price = p.Car.Price,
						Year = p.Car.Year,
						Kilometers = p.Car.Kilometers,
						FuelType = p.Car.FuelType.Name,
						TransmissionType = p.Car.TransmissionType.Name,
						BodyType = p.Car.BodyType.Name,
						LocationTown = p.Car.LocationTown,
						LocationCity = p.Car.LocationCity,
						CoverImage = this.imagesService.GetCoverImagePath(p.Car.Images.ToList()),
					},
					PublishedOn = GetFormattedDate(p.PublishedOn),
					PostID = p.Id
				}).ToList();

			return posts;
		}

		public IEnumerable<PostInListDTO> GetMatchingPosts(SearchCarInputModelDTO searchedCar, int sortingNumber)
		{
			var postsQuery = this.data.Posts.Where(p => !p.IsDeleted && p.IsPublic).AsQueryable();
					
			if (!string.IsNullOrWhiteSpace(searchedCar.TextSearchTerm))
			{
				postsQuery = postsQuery.Where(p =>
					(p.Car.Make.Name + " " + p.Car.CarModel.Name + " " + p.Car.Description).ToLower()
					.Contains(searchedCar.TextSearchTerm.ToLower()));
			}

			if (searchedCar.FromYear is > 0)
			{
				postsQuery = postsQuery.Where(p =>
					p.Car.Year >= searchedCar.FromYear);
			}

			if (searchedCar.ToYear is > 0)
			{
				postsQuery = postsQuery.Where(p =>
					p.Car.Year <= searchedCar.ToYear);
			}

			if (searchedCar.MinEngineCapacity is > 0)
			{
				postsQuery = postsQuery.Where(p =>
					p.Car.EngineCapacity >= searchedCar.MinEngineCapacity);
			}

			if (searchedCar.MaxEngineCapacity is > 0)
			{
				postsQuery = postsQuery.Where(p =>
					p.Car.EngineCapacity <= searchedCar.MaxEngineCapacity);
			}

			if (searchedCar.MinPrice is > 0)
			{
				postsQuery = postsQuery.Where(p =>
					p.Car.Price >= searchedCar.MinPrice);
			}

			if (searchedCar.MaxPrice is > 0)
			{
				postsQuery = postsQuery.Where(p =>
					p.Car.Price <= searchedCar.MaxPrice);
			}

			if (!postsQuery.Any())
			{
				return new List<PostInListDTO>();
			}

			postsQuery = GetSortedPosts(postsQuery, sortingNumber);

			var posts = postsQuery
				.Select(p => new PostInListDTO()
				{
					Car = new CarInListDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Description = p.Car.Description.Length <= 100 ?
							p.Car.Description
							:
							p.Car.Description.Substring(0, 100) + "...",
						Price = p.Car.Price,
						Year = p.Car.Year,
						Kilometers = p.Car.Kilometers,
						FuelType = p.Car.FuelType.Name,
						TransmissionType = p.Car.TransmissionType.Name,
						BodyType = p.Car.BodyType.Name,
						LocationTown = p.Car.LocationTown,
						LocationCity = p.Car.LocationCity,
						CoverImage = this.imagesService.GetCoverImagePath(p.Car.Images.ToList()),
					},
					PublishedOn = GetFormattedDate(p.PublishedOn),
					PostID = p.Id
				}).ToList();

			return posts;
		}

		public int GetAllPostsCount()
		{
			return this.data.Posts.Count();
		}

		public IEnumerable<BasePostInListDTO> GetAllPostsBaseInfo(int page, int postsPerPage, int sort, int filter)
		{
			
			var posts = this.data.Posts				
				.OrderBy(p => p.IsPublic)
				.ThenByDescending(p => p.PublishedOn)
				.Skip((page - 1) * postsPerPage).Take(postsPerPage)
				.Select(p => new BasePostInListDTO()
				{
					Car = new BaseCarDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Year = p.Car.Year,
						Price = p.Car.Price,
					},
					PublishedOn = p.PublishedOn,
					IsPublic = p.IsPublic,
					IsFeatured = p.IsFeatured,
					IsDeleted = p.IsDeleted,
					IsExpired = p.PublishedOn.Date.AddDays(31) < DateTime.Today.Date,
					CanEmptyRecycleBin = p.DeletedOn.HasValue ? p.DeletedOn.Value.Date.AddDays(3) < DateTime.Today.Date: false,
					DeletedOn = p.DeletedOn,
					PostID = p.Id
				}).ToList();

			var wherRslt = filter switch
			{
				1 => posts.Where(p => p.IsDeleted),
				2 => posts.Where(p => p.IsExpired),
				3 => posts.Where(p => p.IsFeatured && ! p.IsDeleted),
				4 => posts.Where(p => p.IsPublic == false && p.IsDeleted == false),
				5 => posts.Where(p => p.IsPublic && ! p.IsDeleted),
				6 => posts.Where(p => p.CanEmptyRecycleBin),
				7 => posts.Where(p => p.PublishedOn.Date == DateTime.Today.Date && p.IsDeleted == false),
				8 => posts.Where(p => p.PublishedOn.Date.AddDays(2) == DateTime.Today.Date && p.IsDeleted == false),
				9 => posts.Where(p => p.PublishedOn.Date.AddDays(3) == DateTime.Today.Date && p.IsDeleted == false),
				10 => posts.Where(p => p.PublishedOn.Date.AddDays(4) == DateTime.Today.Date && p.IsDeleted == false),
				11 => posts.Where(p => p.PublishedOn.Date.AddDays(5) == DateTime.Today.Date && p.IsDeleted == false),
				12 => posts.Where(p => p.PublishedOn.Date.AddDays(6) == DateTime.Today.Date && p.IsDeleted == false),
				13 => posts.Where(p => p.PublishedOn.Date.AddDays(7) == DateTime.Today.Date && p.IsDeleted == false),
				_ => posts.Where(p => p.IsDeleted == false),
			};

			posts = wherRslt.ToList();
			
		var reslt = sort switch
			{
				1 => posts.OrderBy(p => p.PostID),
				2 => posts.OrderByDescending(p => p.Car.Price),
				3 => posts.OrderBy(p => p.Car.Price),				
				6 => posts.OrderByDescending(p => p.Car.Year),
				7 => posts.OrderBy(p => p.Car.Year),
				_ => posts.OrderByDescending(p => p.PostID),
			};

			posts = reslt.ToList();

			return posts;
		}

		public SinglePostDTO GetSinglePostViewModelById(int postId, bool publicOnly = true)
		{
			var post = this.data.Posts
				.Where(p => p.Id == postId && !p.IsDeleted && (!publicOnly || p.IsPublic))
				.Select(p => new SinglePostDTO()
				{
					Car = new SingleCarDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Description = p.Car.Description,
						Price = p.Car.Price,
						Year = p.Car.Year,
						Kilometers = p.Car.Kilometers,
						EngineCapacity = p.Car.EngineCapacity,
						FuelType = p.Car.FuelType.Name,
						TransmissionType = p.Car.TransmissionType.Name,
						BodyType = p.Car.BodyType.Name,
						LocationTown = p.Car.LocationTown,
						LocationCity = p.Car.LocationCity,
						ComfortExtras = p.Car.CarExtras.Where(ce => ce.Extra.TypeId == 1).Select(ce => ce.Extra.Name).ToList(),
						SafetyExtras = p.Car.CarExtras.Where(ce => ce.Extra.TypeId == 2).Select(ce => ce.Extra.Name).ToList(),
						OtherExtras = p.Car.CarExtras.Where(ce => ce.Extra.TypeId == 3).Select(ce => ce.Extra.Name).ToList(),
						Images = p.Car.Images.OrderByDescending(img => img.IsCoverImage)
											 .Select(img => this.imagesService.GetDefaultCarImagesPath(img.Id, img.Extension))
											 .ToList(),
					},
					PublishedOn = GetFormattedDate(p.PublishedOn),
					SellerName = p.SellerName,
					SellerPhoneNumber = p.SellerPhoneNumber,
					SellerEmail = p.SellerEmail,
					PostID = p.Id
				})
				.FirstOrDefault();

			return post;
		}

		public EditPostDTO GetPostFormInputModelById(int postId)
		{
			var post = this.data.Posts
				.Where(p => p.Id == postId && !p.IsDeleted)
				.Select(p => new EditPostDTO()
				{
					Car = new CarFormInputModelDTO()
					{
						MakeId = p.Car.Make.Id,
						CarModelId = p.Car.CarModel.Id,
						Description = p.Car.Description,
						Price = p.Car.Price,
						Year = p.Car.Year,
						Kilometers = p.Car.Kilometers,
						EngineCapacity = p.Car.EngineCapacity,
						BodyTypeId = p.Car.BodyTypeId,
						FuelTypeId = p.Car.FuelTypeId,
						TransmissionTypeId = p.Car.TransmissionTypeId,
						LocationTown = p.Car.LocationTown,
						LocationCity = p.Car.LocationCity,
					},
					PostID = postId,
					SelectedExtrasIds = p.Car.CarExtras.Select(ce => ce.ExtraId).ToList(),
					SellerName = p.SellerName,
					SellerPhoneNumber = p.SellerPhoneNumber,
					SellerEmail = p.SellerEmail,
					CreatorId = p.CreatorId,
					CurrentImages = p.Car.Images.OrderByDescending(img => img.IsCoverImage)
												.Select(img => new ImageInfoDTO()
												{
													Id = img.Id,
													Path = this.imagesService.GetDefaultCarImagesPath(img.Id, img.Extension),
													IsCoverImage = img.IsCoverImage
												}).ToHashSet(),
					SelectedCoverImageId = p.Car.Images.FirstOrDefault(img => img.IsCoverImage).Id,
					CarId = p.CarId,
				}).FirstOrDefault();

			return post;
		}

		public IEnumerable<ImageInfoDTO> GetCurrentDbImagesForAPost(int postId)
		{
			var post = this.data.Posts.FirstOrDefault(p => p.Id == postId && !p.IsDeleted);
			var car = this.data.Cars.FirstOrDefault(c => c.Id == post.CarId && !c.IsDeleted);
			var postImages = this.data.Images
									   .Where(img => img.CarId == car.Id)
									   .OrderByDescending(img => img.IsCoverImage)
									   .Select(img => new ImageInfoDTO()
									   {
										   Id = img.Id,
										   Path = this.imagesService.GetDefaultCarImagesPath(img.Id, img.Extension),
									   }).ToList();

			return postImages;
		}

		public IEnumerable<PostInLatestListDTO> GetLatestExclFeatured(int count)
		{
			try
			{
				var posts = this.data.Posts
				.Where(p => !p.IsDeleted && p.IsPublic && !p.IsFeatured)
				.OrderByDescending(p => p.PublishedOn)
				.Take(count)
				.Select(p => new PostInLatestListDTO()
				{
					Car = new LatestPostsCarDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Price = p.Car.Price,
						Year = p.Car.Year,
						EngineCapacity = p.Car.EngineCapacity,
						FuelType = p.Car.FuelType.Name,
						TransmissionType = p.Car.TransmissionType.Name,
						CoverImage = this.imagesService.GetCoverImagePath(p.Car.Images.ToList())
					},
					PostID = p.Id,
					PublishedOn = GetFormattedDate(p.PublishedOn),
				}).ToList();

				return posts;
			}
			catch (Exception ex)
			{

				throw;
			}



		}

		public IEnumerable<PostInLatestListDTO> GetLatestInclFeatured(int count)
		{
			try
			{
				var posts = this.data.Posts
				.Where(p => !p.IsDeleted && p.IsPublic)
				.OrderByDescending(p => p.PublishedOn)
				.Take(count)
				.Select(p => new PostInLatestListDTO()
				{
					Car = new LatestPostsCarDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Price = p.Car.Price,
						Year = p.Car.Year,
						EngineCapacity = p.Car.EngineCapacity,
						FuelType = p.Car.FuelType.Name,
						TransmissionType = p.Car.TransmissionType.Name,
						CoverImage = this.imagesService.GetCoverImagePath(p.Car.Images.ToList())
					},
					PostID = p.Id,
					PublishedOn = GetFormattedDate(p.PublishedOn),
				}).ToList();

				return posts;
			}
			catch (Exception ex)
			{

				throw;
			}



		}

		/// <summary>
		/// returns a list of 2 items that are the latest, public and featured and not deleted
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SinglePostDTO> GetFeatured()
		{
			try
			{
				var posts = this.data.Posts
				.Where(p => !p.IsDeleted && p.IsPublic && p.IsFeatured)
				.OrderByDescending(p => p.PublishedOn)
				.Take(2)
				.Select(p => new SinglePostDTO()
				{
					Car = new SingleCarDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Description = p.Car.Description,
						Price = p.Car.Price,
						Year = p.Car.Year,
						Kilometers = p.Car.Kilometers,
						EngineCapacity = p.Car.EngineCapacity,
						FuelType = p.Car.FuelType.Name,
						TransmissionType = p.Car.TransmissionType.Name,
						BodyType = p.Car.BodyType.Name,
						LocationTown = p.Car.LocationTown,
						LocationCity = p.Car.LocationCity,
						ComfortExtras = p.Car.CarExtras.Where(ce => ce.Extra.TypeId == 1).Select(ce => ce.Extra.Name).ToList(),
						SafetyExtras = p.Car.CarExtras.Where(ce => ce.Extra.TypeId == 2).Select(ce => ce.Extra.Name).ToList(),
						OtherExtras = p.Car.CarExtras.Where(ce => ce.Extra.TypeId == 3).Select(ce => ce.Extra.Name).ToList(),
						Images = p.Car.Images.OrderByDescending(img => img.IsCoverImage)
											 .Select(img => this.imagesService.GetDefaultCarImagesPath(img.Id, img.Extension))
											 .ToList(),
					},
					PostID = p.Id,
					PublishedOn = GetFormattedDate(p.PublishedOn),
					SellerName = p.SellerName,
					SellerPhoneNumber = p.SellerPhoneNumber,
					SellerEmail = p.SellerEmail
				})
				.ToList();

				return posts;
			}
			catch (Exception ex)
			{

				throw;
			}



		}

		public async Task UpdateAsync(EditPostDTO editedPost, bool isPublic)
		{
			var post = this.GetDbPostById(editedPost.PostID);

			if (post == null)
			{
				throw new Exception($"Unfortunately, we cannot find such post in our system!");
			}

			post.ModifiedOn = DateTime.UtcNow;
			post.SellerName = editedPost.SellerName;
			post.SellerPhoneNumber = editedPost.SellerPhoneNumber;
			post.SellerEmail = editedPost.SellerEmail;
			post.IsPublic = isPublic;

			await this.data.SaveChangesAsync();
		}

		public async Task ChangeVisibilityAsync(int postId)
		{
			var post = this.GetDbPostById(postId);

			if (post == null)
			{
				throw new Exception($"Unfortunately, we cannot find such post in our system!");
			}

			post.IsPublic = ! post.IsPublic;

			await this.data.SaveChangesAsync();
		}

		public async Task SetFeatured(int postId)
		{
			var post = this.GetDbPostById(postId);

			if (post == null)
			{
				throw new Exception($"Unfortunately, we cannot find such post in our system!");
			}

			post.IsFeatured = ! post.IsFeatured;

			await this.data.SaveChangesAsync();
		}

		public PostByUserDTO GetBasicPostInformationById(int postId)
		{
			var post = this.data.Posts
				.Where(p => p.Id == postId && !p.IsDeleted)
				.Select(p => new PostByUserDTO()
				{
					Car = new CarByUserDTO()
					{
						Id = p.Car.Id,
						Make = p.Car.Make.Name,
						CarModel = p.Car.CarModel,
						Price = p.Car.Price,
						Year = p.Car.Year,
						CoverImage = this.imagesService.GetCoverImagePath(p.Car.Images.ToList()),
					},
					PublishedOn = GetFormattedDate(p.PublishedOn),
					PostID = p.Id
				}).FirstOrDefault();

			return post;
		}

		public string GetPostCreatorId(int postId)
		{
			var post = this.GetDbPostById(postId);

			if (post == null)
			{
				throw new Exception($"Unfortunately, we cannot find such post in our system!");
			}

			return post?.CreatorId;
		}

		public async Task DeletePostByIdAsync(int postId)
		{
			using (var transaction = await this.data.Database.BeginTransactionAsync())
			{
				try
				{
					var postExists = this.data.Posts?.ToList().Exists(p => p.Id == postId && !p.IsDeleted);

					if (postExists == null) { postExists = false; }

					if (postExists == false)
					{
						throw new Exception($"Unfortunately, we cannot find such post in our system!");
					}

					var carId = data.Posts?.First(x => x.Id == postId).CarId;

					if (carId == null || carId.Value <= 0)
					{
						throw new Exception($"This post has no car!");
					}

					await this.carsService.DeleteCarByIdAsync(carId.Value, transaction);

					var post = data.Posts!.First(x => x.Id == postId);

					post.IsDeleted = true;
					post.DeletedOn = DateTime.UtcNow;
					post.IsPublic = false;

					await this.data.SaveChangesAsync();

					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		public async Task EmptyRecycleBinAsync(int postId, string imageRootDirectoryPath)
		{
			using (var transaction = await this.data.Database.BeginTransactionAsync())
			{
				try
				{
					if (this.data.Posts is null)
					{
						throw new Exception($"No posts exist!");
					}

					var postExists = this.data.Posts!.ToList().Exists(p => p.Id == postId && p.IsDeleted);									

					if (postExists == false)
					{
						throw new Exception($"Unfortunately, we cannot find such post in our system!");
					}

					var carId = data.Posts?.First(x => x.Id == postId).CarId;

					if (carId == null || carId.Value <= 0)
					{
						throw new Exception($"This post has no car!");
					}
										
					this.data.Posts!.Remove(this.data.Posts.First(x=> x.Id == postId));

					await this.data.SaveChangesAsync();

					await this.carsService.EmptyRecycleBinAsync(carId.Value, imageRootDirectoryPath, transaction);

					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		public async Task RestoreDeletedPost(int postId)
		{
			using (var transaction = await this.data.Database.BeginTransactionAsync())
			{
				try
				{
					var postExists = this.data.Posts?.ToList().Exists(p => p.Id == postId && p.IsDeleted);

					if (postExists == null) { postExists = false; }

					if (postExists == false)
					{
						throw new Exception($"Unfortunately, we cannot find such post in our system!");
					}

					var carId = data.Posts?.First(x => x.Id == postId).CarId;

					if (carId == null || carId.Value <= 0)
					{
						throw new Exception($"This post has no car!");
					}

					await this.carsService.RestoreDeletedCar(carId.Value, transaction);

					var post = data.Posts!.First(x => x.Id == postId);

					post.IsDeleted = false;
					post.DeletedOn = null;
					post.IsPublic = true;

					await this.data.SaveChangesAsync();

					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}


		private Post? GetDbPostById(int postId)
		{
			return this.data.Posts.FirstOrDefault(p => p.Id == postId && !p.IsDeleted);
		}

		private static string GetFormattedDate(DateTime? inputDateTime)
		{
			if (inputDateTime == null)
			{
				return "";
			}

			var inputDate = inputDateTime.Value;

			if (inputDate.Date == DateTime.UtcNow.Date)
			{
				return "Today, " + inputDate.ToString("t", CultureInfo.InvariantCulture);
			}

			return inputDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
		}

		private static IQueryable<Post> GetSortedPosts(IQueryable<Post> postsQuery, int sortingNumber)
		{
			postsQuery = sortingNumber switch
			{
				1 => postsQuery.OrderBy(p => p.Id),
				2 => postsQuery.OrderByDescending(p => p.Car.Price),
				3 => postsQuery.OrderBy(p => p.Car.Price),
				4 => postsQuery.OrderByDescending(p => p.Car.EngineCapacity),
				5 => postsQuery.OrderBy(p => p.Car.EngineCapacity),
				6 => postsQuery.OrderByDescending(p => p.Car.Year),
				7 => postsQuery.OrderBy(p => p.Car.Year),
				_ => postsQuery.OrderByDescending(p => p.Id),
			};

			return postsQuery;
		}

		
	}
}