using System.Collections.Generic;

namespace CashRegister
{
    /// <summary>
    ///  Active Record Pattern for easy manipulation of models.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IActiveRecord<T>
    {
        /// <summary>
        ///  ActiveRecord ALL.
        /// </summary>
        /// <returns>Returns every item in the collection.</returns>
        Dictionary<string, T> All();

        /// <summary>
        ///  ActiveRecord CREATE.
        /// </summary>
        /// <param name="id">Unique id to use as key.</param>
        /// <param name="obj">Object to store in collection.</param>
        void Create(string id, T obj);

        /// <summary>
        ///  ActiveRecord GET
        /// </summary>
        /// <param name="id">Unique id to search for.</param>
        /// <returns>Returns an object from the collection.</returns>
        T Get(string id);

        /// <summary>
        ///  ActiveRecord DELETE
        /// </summary>
        /// <param name="id">Unique id of object to delete.</param>
        void Delete(string id);
    }
}