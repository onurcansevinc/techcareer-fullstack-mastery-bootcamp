using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techcareer_fullstack_mastery_bootcamp.Data;
using techcareer_fullstack_mastery_bootcamp.Models;

namespace techcareer_fullstack_mastery_bootcamp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookContext _context;
        private readonly int _pageSize = 12; // Her sayfada 12 kitap gösterilecek

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString, string author, string genre, string sortOrder, int? pageNumber)
        {
            var booksQuery = _context.Books.AsQueryable();

            // Arama filtreleri
            if (!string.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(author))
            {
                booksQuery = booksQuery.Where(b => b.Author.Contains(author));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                booksQuery = booksQuery.Where(b => b.Genre == genre);
            }

            // Sıralama
            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["PagesSortParm"] = sortOrder == "pages" ? "pages_desc" : "pages";

            switch (sortOrder)
            {
                case "title_desc":
                    booksQuery = booksQuery.OrderByDescending(b => b.Title);
                    break;
                case "author":
                    booksQuery = booksQuery.OrderBy(b => b.Author);
                    break;
                case "author_desc":
                    booksQuery = booksQuery.OrderByDescending(b => b.Author);
                    break;
                case "pages":
                    booksQuery = booksQuery.OrderBy(b => b.PageCount);
                    break;
                case "pages_desc":
                    booksQuery = booksQuery.OrderByDescending(b => b.PageCount);
                    break;
                default:
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
            }

            // Mevcut filtreleri ViewData'ya ekle
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentAuthor"] = author;
            ViewData["CurrentGenre"] = genre;
            ViewData["CurrentSort"] = sortOrder;

            // Tüm türleri getir
            ViewData["Genres"] = await _context.Books.Select(b => b.Genre).Distinct().ToListAsync();

            // Sayfalama
            return View(await PaginatedList<Book>.CreateAsync(booksQuery, pageNumber ?? 1, _pageSize));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var similarBooks = await _context.Books
                .Where(b => b.Genre == book.Genre && b.Id != book.Id)
                .Take(3)
                .ToListAsync();

            ViewData["SimilarBooks"] = similarBooks;

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Description,PageCount,Genre")] Book book, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            try
            {
                // Görsel işleme
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Görsel için klasör yolu
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "books");
                    
                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Benzersiz dosya adı
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Dosyayı kaydet
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Veritabanı için yolu kaydet
                    book.ImagePath = $"/images/books/{uniqueFileName}";
                }

                // Kitabı kaydet
                _context.Add(book);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Kitap başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kitap eklenirken bir hata oluştu: " + ex.Message);
                return View(book);
            }
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Description,PageCount,Genre,ImagePath")] Book book, IFormFile? imageFile)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mevcut kitabı getir (mevcut resim yolunu korumak için)
                    var existingBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    // Yeni görsel yüklendiyse
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Eski görseli sil (varsa)
                        if (!string.IsNullOrEmpty(existingBook.ImagePath))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingBook.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Yeni görseli kaydet
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "books");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        book.ImagePath = $"/images/books/{uniqueFileName}";
                    }
                    else
                    {
                        // Yeni görsel yüklenmediyse mevcut görsel yolunu koru
                        book.ImagePath = existingBook.ImagePath;
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Kitap başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kitap başarıyla silindi!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
} 