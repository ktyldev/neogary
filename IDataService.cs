using System;
using System.Data;

public interface IDataService
{
    void Find(string table, string condition, Action<IDataRecord> readRecord);
    int Insert(string table, string columns, string values);
    int Update(string table, string setValues, string condition);
}
