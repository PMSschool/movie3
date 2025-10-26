//using System.Linq.Expressions;
//using System.Threading;

//namespace ECommerce.Repositories
//{
//    public interface IRepository<T> where T : class
//    {
//        // إضافة عنصر جديد
//        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

//        // تحديث عنصر
//        void Update(T entity);

//        // حذف عنصر
//        void Delete(T entity);

//        // جلب كل العناصر مع إمكانية الفلترة والتضمين (Include)
//        Task<IEnumerable<T>> GetAsync(
//            Expression<Func<T, bool>>? expression = null,
//            Expression<Func<T, object>>[]? includes = null,
//            bool tracked = true,
//            CancellationToken cancellationToken = default);

//        // جلب عنصر واحد فقط
//        Task<T?> GetOneAsync(
//            Expression<Func<T, bool>>? expression = null,
//            Expression<Func<T, object>>[]? includes = null,
//            bool tracked = true,
//            CancellationToken cancellationToken = default);

//        // حفظ التغييرات في قاعدة البيانات
//        Task CommitAsync(CancellationToken cancellationToken = default);
//    }
//}
