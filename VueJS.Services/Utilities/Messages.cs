using VueJS.Entities.ComplexTypes;

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

        public static string NotFound(ObjectType objectType, bool isPlural)
        {
            switch (objectType)
            {
                case ObjectType.page:
                    if (isPlural) return "Hiçbir sayfa bulunamadı.";
                    return "Böyle bir sayfa bulunamadı.";
                case ObjectType.article:
                    if (isPlural) return "Hiçbir makale bulunamadı.";
                    return "Böyle bir makale bulunamadı.";
                case ObjectType.category:
                    if (isPlural) return "Hiçbir kategori bulunamadı.";
                    return "Böyle bir kategori bulunamadı.";
                case ObjectType.tag:
                    if (isPlural) return "Hiçbir etiket bulunamadı.";
                    return "Böyle etiket bulunamadı.";
                case ObjectType.basepage:
                    if (isPlural) return "Hiçbir temel sayfa bulunamadı.";
                    return "Böyle bir temel sayfa bulunamadı.";
                default:
                    if (isPlural) return "Hiçbir gönderi bulunamadı.";
                    return "Böyle bir gönderi bulunamadı.";
            }
        }

        public static string UrlCheck(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.page:
                    return "Bu sayfanın linki zaten mevcut, lütfen sayfanın linkini değiştiriniz.";
                case ObjectType.article:
                    return "Bu makalenin linki zaten mevcut, lütfen makalenin linkini değiştiriniz.";
                case ObjectType.category:
                    return "Bu kategorinin kısa ismi zaten mevcut, lütfen kategorinin kısa ismini değiştiriniz.";
                case ObjectType.tag:
                    return "Bu etiketin kısa ismi zaten mevcut, lütfen etiketin kısa ismini değiştiriniz.";
                case ObjectType.basepage:
                    return "Bu temel sayfanın linki zaten mevcut, lütfen temel sayfanın linkini değiştiriniz.";
                default:
                    return "Bu gönderinin linki zaten mevcut, lütfen gönderinin linkini değiştiriniz.";
            }
        }

        public static string Add(ObjectType objectType, string objectTitle)
        {
            switch (objectType)
            {
                case ObjectType.page:
                    return $"{objectTitle} başlıklı sayfa eklendi.";
                case ObjectType.article:
                    return $"{objectTitle} başlıklı makale eklendi.";
                case ObjectType.category:
                    return $"{objectTitle} başlıklı kategori eklendi.";
                case ObjectType.tag:
                    return $"{objectTitle} başlıklı etiket eklendi.";
                case ObjectType.basepage:
                    return $"{objectTitle} başlıklı temel sayfa eklendi.";
                default:
                    return $"{objectTitle} başlıklı gönderi eklendi.";
            }
        }

        public static string Update(ObjectType objectType, string objectTitle)
        {
            switch (objectType)
            {
                case ObjectType.page:
                    return $"{objectTitle} başlıklı sayfa güncellendi.";
                case ObjectType.article:
                    return $"{objectTitle} başlıklı makale güncellendi.";
                case ObjectType.category:
                    return $"{objectTitle} başlıklı kategori güncellendi.";
                case ObjectType.tag:
                    return $"{objectTitle} başlıklı etiket güncellendi.";
                case ObjectType.basepage:
                    return $"{objectTitle} başlıklı temel sayfa güncellendi.";
                default:
                    return $"{objectTitle} başlıklı gönderi güncellendi.";
            }
        }

        public static string StatusChange(ObjectType objectType , PostStatus postStatus, string objectTitle)
        {
            switch (objectType)
            {
                case ObjectType.page:
                    if (postStatus == PostStatus.publish)
                        return $"{objectTitle} başlıklı sayfa yayınlandı.";
                    else if (postStatus == PostStatus.draft)
                        return $"{objectTitle} başlıklı sayfa taslak olarak kaydedildi.";
                    else
                        return $"{objectTitle} başlıklı sayfa çöp kutusuna taşındı.";
                case ObjectType.article:
                    if (postStatus == PostStatus.publish)
                        return $"{objectTitle} başlıklı makale yayınlandı.";
                    else if (postStatus == PostStatus.draft)
                        return $"{objectTitle} başlıklı makale taslak olarak kaydedildi.";
                    else
                        return $"{objectTitle} başlıklı makale çöp kutusuna taşındı.";
                case ObjectType.basepage:
                    if (postStatus == PostStatus.publish)
                        return $"{objectTitle} başlıklı sayfa yayınlandı.";
                    else if (postStatus == PostStatus.draft)
                        return $"{objectTitle} başlıklı temel sayfa taslak olarak kaydedildi.";
                    else
                        return $"{objectTitle} başlıklı sayfa çöp kutusuna taşındı.";
                default:
                    if (postStatus == PostStatus.publish)
                        return $"{objectTitle} başlıklı gönderi çöp kutusuna taşındı.";
                    else if (postStatus == PostStatus.draft)
                        return $"{objectTitle} başlıklı gönderi taslak olarak kaydedildi.";
                    else
                        return $"{objectTitle} başlıklı gönderi çöp kutusuna taşındı.";
            }
        }

        public static string Delete(ObjectType objectType, string objectTitle)
        {
            switch (objectType)
            {
                case ObjectType.page:
                    return $"{objectTitle} başlıklı sayfa kalıcı olarak silindi.";
                case ObjectType.article:
                    return $"{objectTitle} başlıklı makale kalıcı olarak silindi.";
                case ObjectType.category:
                    return $"{objectTitle} başlıklı kategori silindi.";
                case ObjectType.tag:
                    return $"{objectTitle} başlıklı etiket silindi.";
                case ObjectType.basepage:
                    return $"{objectTitle} başlıklı temel sayfa kalıcı olarak silindi.";
                default:
                    return $"{objectTitle} başlıklı gönderi kalıcı olarak silindi.";
            }
        }


        public static class BasePage
        {
            public static string NoDelete()
            {
                return "Temel sayfalar silinemez. Pasif moda almak için taslak olarak kaydedebilirsiniz.";
            }
        }

        public static class Comment
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiçbir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }

            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz eklendi.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum güncellendi.";
            }

            public static string CommentStatusChange(CommentStatus commentStatus, int commentId)
            {
                if (commentStatus == CommentStatus.approved)
                    return $"{commentId} no'lu yorum onaylandı.";
                else if (commentStatus == CommentStatus.moderated)
                    return $"{commentId} no'lu yorum onay için bekleyeme alındı.";
                else
                    return $"{commentId} no'lu yorum çöp kutusuna gönderildi.";
            }

            public static string Delete()
            {
                return "Yorum kalıcı olarak silindi.";
            }

        }

        public static class Upload
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiçbir medya dosyası bulunamadı.";
                return "Böyle bir medya dosyası bulunamadı.";
            }

            public static string Add(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası eklendi.";
            }

            public static string Update(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası güncellendi.";
            }

            public static string Delete(string uploadFileName)
            {
                return $"{uploadFileName} adlı medya dosyası silindi.";
            }
        }

        public static class UrlRedirect
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiçbir yönlendirme bulunamadı.";
                return "Böyle bir yönlendirme bulunamadı.";
            }

            public static string Add(string urlRedirectName)
            {
                return $"{urlRedirectName} yönlendirme eklendi.";
            }

            public static string UrlCheck()
            {
                return "Bu link zaten daha önce yönlendirilmiş. Lütfen kontrol ediniz.";
            }

            public static string Update(string urlRedirectName)
            {
                return $"{urlRedirectName} yönlendirme güncellendi.";
            }

            public static string Delete(string urlRedirectName)
            {
                return $"{urlRedirectName} yönlendirme silindi.";
            }

        }

        public static class Menu
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiçbir menu bulunamadı.";
                return "Böyle bir menu bulunamadı.";
            }

            public static string TitleCheck()
            {
                return "Bu menü başlığı zaten mevcut, lütfen başlığı değiştiriniz.";
            }

            public static string Add(string menuName)
            {
                return $"{menuName} adlı menü eklendi.";
            }

            public static string Update(string menuName)
            {
                return $"{menuName} adlı menü güncellendi.";
            }

            public static string Delete(string menuName)
            {
                return $"{menuName} adlı menü silindi.";
            }
        }

        public static class MenuDetail
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiçbir menu içeriği bulunamadı.";
                return "Böyle bir menu içeriği bulunamadı.";
            }

            public static string Add(string menuDetailName)
            {
                return $"{menuDetailName} adlı menu içeriği eklendi.";
            }

            public static string Update(string menuDetailName)
            {
                return $"{menuDetailName} adlı menu içeriği güncellendi.";
            }

            public static string Delete(string menuDetailName)
            {
                return $"{menuDetailName} adlı menu içeriği silindi.";
            }
        }
    }
}