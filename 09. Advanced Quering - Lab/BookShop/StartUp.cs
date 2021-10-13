namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db,"miNor"));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db,2000));
            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, "dy"));
            //Console.WriteLine(GetBookTitlesContaining(db,"sK"));
            //Console.WriteLine(GetBooksByAuthor(db,"po"));
            //Console.WriteLine(CountBooks(db,12));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetTotalProfitByCategory(db));
            //Console.WriteLine(GetMostRecentBooks(db));
            //IncreasePrices(db);
            //Console.WriteLine(RemoveBooks(db));


        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(x => x.Copies < 4200).ToList();
            context.Books.RemoveRange(books);
            context.SaveChanges();
            return books.Count;

        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010).ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
                   
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();
            var books = context.Categories.Select(x => new
            {
                CategoryName = x.Name,
                RecentBooks = x.CategoryBooks.Select(c => new
                {
                    BookTitle = c.Book.Title,
                    BookYear = c.Book.ReleaseDate.Value
                })
                .OrderByDescending(c => c.BookYear)
                .Take(3)
                .ToList()
            })
                .OrderBy(x => x.CategoryName)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"--{book.CategoryName}");

                foreach (var bookInCategory in book.RecentBooks)
                {
                    sb.AppendLine($"{bookInCategory.BookTitle} ({bookInCategory.BookYear.Year})");
                }
            }

            return sb.ToString().TrimEnd();


        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Categories.Select(x => new
            {
                CategoryName = x.Name,
                TotalProfit = x.CategoryBooks.Sum(s=>s.Book.Copies*s.Book.Price)
                
            })
            .OrderByDescending(x => x.TotalProfit)
            .ThenBy(x => x.CategoryName)
            .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.CategoryName} ${book.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var bookCoppies = context.Authors
                                     .Select(x => new
                                     {
                                         FullName = x.FirstName + " " + x.LastName,
                                         CountFullCopies = x.Books.Sum(c => c.Copies)
                                        
                                     })
                                     .OrderByDescending(x => x.CountFullCopies)
                                     .ToList();

            foreach (var bk in bookCoppies)
            {
                sb.AppendLine($"{bk.FullName} - {bk.CountFullCopies}");
            }

            return sb.ToString().TrimEnd();

        }


        public static int CountBooks(BookShopContext context, int lengthCheck)
        {

            var books = context.Books.Where(x => x.Title.Length > lengthCheck)
                .Count();

            return books;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            var books = context.Books.Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                               .Select(x => new
                               {
                                   x.Title,
                                   x.BookId,
                                   AuthorFullName = x.Author.FirstName + " " + x.Author.LastName
                               })
                               .OrderBy(x => x.BookId)
                               .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books.Where(x => x.Title.ToLower().Contains(input.ToLower()))
                               .Select(x => x.Title)
                               .OrderBy(x=>x)
                               .ToList();


            var result = string.Join(Environment.NewLine, books);
            return result;

        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {

            var authors = context.Authors.Where(x => x.FirstName.Substring
            (x.FirstName.Length - input.Length, input.Length) == input)
                                         .Select(x => new
                                         {
                                             FullName = x.FirstName + " " + x.LastName
                                         })
                                         .OrderBy(x=>x.FullName)
                                         .ToList();


            var result = string.Join(Environment.NewLine, authors.Select(x=>x.FullName));
            return result;

        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();

            var formattedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books.Where(x => x.ReleaseDate < formattedDate)
                               .Select(x => new
                               {
                                   x.Title,
                                   x.EditionType,
                                   x.Price,
                                   x.ReleaseDate
                               })
                               .OrderByDescending(x => x.ReleaseDate)
                               .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }



        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                  .Select(x=>x.ToLower());
            var books = context.Books.Where(x => x.BookCategories.All(c => categories.Contains(c.Category.Name.ToLower())))
                              .Select(x => x.Title)
                              .OrderBy(x => x)
                              .ToList();

            var result = string.Join(Environment.NewLine, books);

            return result;
         
        }


        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
           
            //var inputCommand = Enum.Parse(typeof(AgeRestriction), command, true);
            var inputCommand = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books.Where(x => x.AgeRestriction == inputCommand)
                                .Select(x => x.Title)
                                .OrderBy(x => x)
                                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetGoldenBooks(BookShopContext context)
        {

            var goldenBooks = context.Books.Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                            .Select(x => new 
                            { 
                              x.BookId,
                              x.Title
                            })
                            .OrderBy(x => x.BookId)
                            .ToList();


            var result = string.Join(Environment.NewLine, goldenBooks.Select(x=>x.Title));

            return result;


        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books.Where(x => x.Price > 40)
                               .Select(x => new
                               {
                                   x.Title,
                                   x.Price
                               })
                               .OrderByDescending(x => x.Price)
                               .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var books = context.Books.Where(x => x.ReleaseDate.Value.Year != year)
                                     .Select(x => new
                                     {
                                         x.BookId,
                                         x.Title
                                     })
                                     .OrderBy(x => x.BookId)
                                     .ToList();


            var result = string.Join(Environment.NewLine, books.Select(x => x.Title));

            return result;


        }
    }
}
