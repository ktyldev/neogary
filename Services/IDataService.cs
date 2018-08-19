using System;
using System.Data;

public interface IDataService
{
    int Find(string table, string condition, Action<IDataRecord> readRecord);
    int Insert(string table, string columns, string values);
    int Update(string table, string setValues, string condition);
    int Remove(string table, string condition);
}
