namespace VueJS.Services.Utilities
{
    public static class Messages
    {
        public static class General
        {
            public static string ValidationError()
            {
                return "Bir veya daha fazla validasyon hatası ile karşılaşıldı.";
            }
        }

        public static class User
        {
            public static string NotFoundById(int userId)
            {
                return $"{userId} kullanıcı koduna ait bir kullanıcı bulunamadı.";
            }
        }

        public static class Category
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir kategori bulunamadı.";
                return "Böyle bir kategori bulunamadı.";
            }

            public static string NotFoundById(int categoryId)
            {
                return $"{categoryId} kategori koduna ait bir kategori bulunamadı.";
            }

            public static string UrlCheck()
            {
                return "Bu kategori adı zaten mevcut, lütfen adını değiştiriniz.";
            }

            public static string Add(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla eklenmiştir.";
            }

            public static string Update(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla güncellenmiştir.";
            }

            public static string Delete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla silinmiştir.";
            }

            public static string HardDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Tag
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir etiket bulunamadı.";
                return "Böyle bir etiket bulunamadı.";
            }

            public static string NotFoundById(int tagId)
            {
                return $"{tagId} etiket koduna ait bir etiket bulunamadı.";
            }

            public static string UrlCheck()
            {
                return "Bu etiket adı zaten mevcut, lütfen adını değiştiriniz.";
            }

            public static string Add(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla eklenmiştir.";
            }

            public static string Update(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla güncellenmiştir.";
            }

            public static string Delete(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla silinmiştir.";
            }

            public static string HardDelete(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string tagName)
            {
                return $"{tagName} adlı etiket başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Article
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Makaleler bulunamadı.";
                return "Böyle bir makale bulunamadı.";
            }

            public static string NotFoundById(int articleId)
            {
                return $"{articleId} makale koduna ait bir makale bulunamadı.";
            }

            public static string UrlCheck()
            {
                return "Bu makale adı zaten mevcut, lütfen başlığı değiştiriniz.";
            }

            public static string Add(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla eklenmiştir.";
            }

            public static string Update(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla güncellenmiştir.";
            }

            public static string Delete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla silinmiştir.";
            }

            public static string HardDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla arşivden geri getirilmiştir.";
            }

            public static string IncreaseViewCount(string articleTitle)
            {
                return $"{articleTitle} başlıklı makalenin okunma sayısı  başarıyla attırılmıştır.";
            }
        }

        public static class Page
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Sayfalar bulunamadı.";
                return "Böyle bir sayfa bulunamadı.";
            }

            public static string Add(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla eklenmiştir.";
            }

            public static string Update(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla güncellenmiştir.";
            }

            public static string Delete(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla silinmiştir.";
            }

            public static string HardDelete(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string pageTitle)
            {
                return $"{pageTitle} başlıklı sayfa başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Comment
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }

            public static string Approve(int commentId)
            {
                return $"{commentId} no'lu yorum onaylandı.";
            }

            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz eklendi.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum güncellendi.";
            }

            public static string Trash()
            {
                return "Yorum çöp kutusuna gönderildi.";
            }

            public static string MultiTrash(int count)
            {
                return $"{count} adet yorum çöp kutusuna gönderildi.";
            }

            public static string UnTrash()
            {
                return "Yorum çöp kutusundan geri alındı.";
            }

            public static string MultiUnTrash(int count)
            {
                return $"{count} adet yorum çöp kutusundan geri alındı.";
            }

            public static string Delete()
            {
                return "Yorum kalıcı olarak silindi.";
            }

            public static string MultiDelete(int count)
            {
                return $"{count} adet yorum kalıcı olarak silindi.";
            }
        }

        public static class Upload
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir medya dosyası bulunamadı.";
                return "Böyle bir medya dosyası bulunamadı.";
            }

            public static string Add(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası başarıyla eklenmiştir.";
            }

            public static string Update(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası başarıyla güncellenmiştir.";
            }

            public static string Delete(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası başarıyla silinmiştir.";
            }
        }

        public static class UrlRedirect
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir url bulunamadı.";
                return "Böyle bir kategori bulunamadı.";
            }

            public static string NotFoundById(int urlRedirectId)
            {
                return $"{urlRedirectId} url koduna ait bir kategori bulunamadı.";
            }

            public static string Add(string urlRedirectName)
            {
                return $"{urlRedirectName} url başarıyla eklenmiştir.";
            }

            public static string Update(string urlRedirectName)
            {
                return $"{urlRedirectName} url başarıyla güncellenmiştir.";
            }

            public static string Delete(string urlRedirectName)
            {
                return $"{urlRedirectName} url başarıyla silinmiştir.";
            }

            public static string HardDelete(string urlRedirectName)
            {
                return $"{urlRedirectName} url başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string urlRedirectName)
            {
                return $"{urlRedirectName} url başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Product
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Ürünler bulunamadı.";
                return "Böyle bir ürün bulunamadı.";
            }

            public static string NotFoundById(int productId)
            {
                return $"{productId} koduna ait bir ürün bulunamadı.";
            }

            public static string UrlCheck()
            {
                return "Bu ürün adı zaten mevcut, lütfen başlığı değiştiriniz.";
            }

            public static string Add(string productTitle)
            {
                return $"{productTitle} başlıklı ürün başarıyla eklenmiştir.";
            }

            public static string Update(string productTitle)
            {
                return $"{productTitle} başlıklı ürün başarıyla güncellenmiştir.";
            }

            public static string Delete(string productTitle)
            {
                return $"{productTitle} başlıklı ürün başarıyla silinmiştir.";
            }

            public static string HardDelete(string productTitle)
            {
                return $"{productTitle} başlıklı ürün başarıyla veritabanından silinmiştir.";
            }

            public static string UndoDelete(string productTitle)
            {
                return $"{productTitle} başlıklı ürün başarıyla arşivden geri getirilmiştir.";
            }
        }

        public static class Post
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Gönderi bulunamadı.";
                return "Böyle bir gönderi bulunamadı.";
            }

            public static string NotFoundById(int productId)
            {
                return $"{productId} koduna ait bir gönderi bulunamadı.";
            }

            public static string BasePage()
            {
                return "Temel sayfalar silinemez.";
            }

            public static string TitleCheck()
            {
                return "Bu gönderi başlığı zaten mevcut, lütfen başlığı değiştiriniz.";
            }

            public static string UrlCheck()
            {
                return "Bu gönderi linki zaten mevcut, lütfen linki değiştiriniz.";
            }

            public static string Add(string productTitle)
            {
                return $"{productTitle} başlıklı gönderi eklendi.";
            }

            public static string Update(string productTitle)
            {
                return $"{productTitle} başlıklı gönderi güncellendi.";
            }

            public static string Trash(string productTitle)
            {
                return $"{productTitle} başlıklı gönderi çöp kutusuna gönderildi.";
            }

            public static string MultiTrash(int count)
            {
                return $"{count} adet gönderi çöp kutusuna gönderildi.";
            }

            public static string UnTrash(string productTitle)
            {
                return $"{productTitle} başlıklı gönderi çöp kutusundan geri getirildi.";
            }

            public static string MultiUnTrash(int count)
            {
                return $"{count} adet gönderi çöp kutusundan geri alındı.";
            }

            public static string Delete(string productTitle)
            {
                return $"{productTitle} başlıklı gönderi kalıcı olarak silindi.";
            }

            public static string MultiDelete(int count)
            {
                return $"{count} adet gönderi kalıcı olarak silindi.";
            }

            public static string IncreaseViewCount(string articleTitle)
            {
                return $"{articleTitle} başlıklı gönderinin okunma sayısı attırıldı.";
            }
        }

        public static class Term
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir terim bulunamadı.";
                return "Böyle bir terim bulunamadı.";
            }

            public static string NotFoundById(int termId)
            {
                return $"{termId} koduna ait bir terim bulunamadı.";
            }

            public static string NameCheck()
            {
                return "Bu terim adı zaten mevcut, lütfen adını değiştiriniz.";
            }

            public static string UrlCheck()
            {
                return "Bu terim linki zaten mevcut, lütfen linki değiştiriniz.";
            }

            public static string Add(string termName)
            {
                return $"{termName} adlı terim eklendi.";
            }

            public static string Update(string termName)
            {
                return $"{termName} adlı terim güncellendi.";
            }

            public static string Delete(string termName)
            {
                return $"{termName} adlı terim silindi.";
            }
        }
    }
}