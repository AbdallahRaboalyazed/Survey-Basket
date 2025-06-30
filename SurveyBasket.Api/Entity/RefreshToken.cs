namespace SurveyBasket.Entity;

[Owned]
public class RefreshToken
{  
        // التوكن الذي سيتم إرساله للمستخدم لتحديث الـ Access Token
        public string Token { get; set; } = string.Empty;

        // تاريخ انتهاء صلاحية التوكن
        public DateTime ExpiresOn { get; set; }

        // وقت إنشاء التوكن عند إضافته لأول مرة في قاعدة البيانات
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // وقت إبطال التوكن (إذا تم إلغاؤه يدويًا)، وإلا يكون null
        public DateTime? RevokedOn { get; set; }

        // هل التوكن منتهي الصلاحية؟
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

        // هل التوكن لا يزال صالحًا؟ (لم يتم إبطاله ولم ينتهِ بعد)
        public bool IsActive => RevokedOn is null && !IsExpired;

}
