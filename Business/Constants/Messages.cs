using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string Added = "Eklendi";
        public static string CarNameInValıd = "Araba açıklaması en az 2 karakterden oluşmalıdır.\nGünlük fiyatı 0 liradan büyük olmalıdır.";
        public static string Deleted = "Silindi";
        public static string Updated = "Güncellendi";
        public static string Listed = "Listelendi";
        public static string NotDelivered = "Bu araba henüz teslim edilmedi";
        public static string RentalAdded = "Araba kiralanması eklendi";
        public static string CarImageLımıtError = "Bir arabanın en fazla 5 resmi olabilir.";
        public static string AuthorizationDenied = "Yetkiniz yok";
        public static string UserRegistered="Kayıt oldu";
        public static string UserNotFound="Kullanıcı bulunamadı";
        public static string PasswordError="Parola hatası";
        public static string SuccessfulLogin="Başarılı giriş";
        public static string UserAlreadyExists="Kullanıcı mevcut";
        public static string AccessTokenCreated="Token oluşturuldu";
    }
}
