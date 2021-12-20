﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class EmailSendDto
    {
        [DisplayName("Adınız ve Soyadınız")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MaxLength(60, ErrorMessage = "{0} alanı en fazla {1} karakterden oluşmalıdır.")]
        [MinLength(5, ErrorMessage = "{0} alanı en az {1} karakterden oluşmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Telefon Numaranız")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MaxLength(100, ErrorMessage = "{0} alanı en fazla {1} karakterden oluşmalıdır.")]
        [MinLength(10, ErrorMessage = "{0} alanı en az {1} karakterden oluşmalıdır.")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [DisplayName("E-Posta Adresiniz")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MaxLength(100, ErrorMessage = "{0} alanı en fazla {1} karakterden oluşmalıdır.")]
        [MinLength(10, ErrorMessage = "{0} alanı en az {1} karakterden oluşmalıdır.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Konu")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MaxLength(125, ErrorMessage = "{0} alanı en fazla {1} karakterden oluşmalıdır.")]
        [MinLength(5, ErrorMessage = "{0} alanı en az {1} karakterden oluşmalıdır.")]
        public string Subject { get; set; }

        [DisplayName("Mesajınız")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MaxLength(1500, ErrorMessage = "{0} alanı en fazla {1} karakterden oluşmalıdır.")]
        [MinLength(20, ErrorMessage = "{0} alanı en az {1} karakterden oluşmalıdır.")]
        public string Message { get; set; }
    }
}