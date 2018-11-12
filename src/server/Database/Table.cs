using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Hostel.Server.Models;
using Hostel.Server.Services;

namespace Hostel.Server.Database
{

    public abstract class Table<T>
        where T : class, IRecord
    {

        public enum ErrorState
        {
            Ok,
            NothingProvided,
            NothingFound,
            NotAllFieldsProvided

        }

        protected DatabaseService DbService;

        protected abstract DbSet<T> GetDbSet(ApplicationContext db);

        public T GetById(int id)
        {
            T result;
            using (var db = DbService.Context)
            {
                var table = this.GetDbSet(db);
                result = table.Where(r => r.Id == id).FirstOrDefault();
            }
            return result;
        }

        public abstract bool IsRequiredFieldsSet(T item);

        public (T, ErrorState) RequestAdd(String content)
        {
            T item = JsonConvert.DeserializeObject<T>(content);

            if (item == null)
            {
                return (null, ErrorState.NothingProvided);
            }

            bool isRequiredSet = this.IsRequiredFieldsSet(item);
            if (!isRequiredSet)
            {
                return (null, ErrorState.NotAllFieldsProvided);
            }

            using (var db = this.DbService.Context)
            {
                var table = this.GetDbSet(db);
                table.Add(item);
                db.SaveChanges();
            }

            return (item, ErrorState.Ok);
        }

        public (T, ErrorState) RequestRemove(int id)
        {

            T item = this.GetById(id);

            if (item == null)
            {
                return (null, ErrorState.NothingFound);
            }

            using (var db = this.DbService.Context)
            {
                this.GetDbSet(db).Remove(item);
                db.SaveChanges();
            }

            return (null, ErrorState.Ok);
        }

        public (T, ErrorState) RequestGet(int id)
        {
            T result = this.GetById(id);

            if (result == null)
            {
                return (null, ErrorState.NothingFound);
            }

            return (result, ErrorState.Ok);
        }

        public (T, ErrorState) RequestUpdate(int id, string content)
        {
            T newData = JsonConvert.DeserializeObject<T>(content);

            if (newData == null)
            {
                return (null, ErrorState.NothingProvided);
            }

            newData.Id = id;

            using (var db = this.DbService.Context)
            {
                this.GetDbSet(db).Update(newData);
                db.SaveChanges();
            }

            return (newData, ErrorState.Ok);
        }

        public (List<T>, ErrorState) RequestGetAll()
        {
            List<T> result;
            using (var db = this.DbService.Context)
            {
                result = this.GetDbSet(db).ToList();
            }

            return (result, ErrorState.Ok);
        }

    }

}