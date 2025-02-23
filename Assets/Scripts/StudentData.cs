using System;
using System.Collections.Generic;

[Serializable]
public class StudentData 
{
    public string name;
    public string lastName;
    public float attendancePercentage;
    public float absence;
    public float attendances;
    public List <Note> notes;
    public float noteAverage;
    public float notePercentage;
    public DataCourse course;
    public string initialCourse;
    public List<DiaInfoEntry> historialDiasList = new List<DiaInfoEntry>();

}
[Serializable]
public class JsonData
{
    public List<StudentData> students;
}

[Serializable]
public class Note
{
    public string referencia;
    public float value;
}
[Serializable]
public class DiaInfoEntry
{
    public string key;
    public DiaInfo value;

    public DiaInfoEntry(string key, DiaInfo value)
    {
        this.key = key;
        this.value = value;
    }
}
public static class DictionaryConverter
{
    public static List<DiaInfoEntry> ConvertDictionaryToDiaInfoEntries(Dictionary<string, DiaInfo> dictionary)
    {
        List<DiaInfoEntry> entries = new List<DiaInfoEntry>();

        foreach (KeyValuePair<string, DiaInfo> kvp in dictionary)
        {
            entries.Add(new DiaInfoEntry(kvp.Key, kvp.Value));
        }

        return entries;
    }

    public static Dictionary<string, DiaInfo> ConvertDiaInfoEntriesToDictionary(List<DiaInfoEntry> entries)
    {
        Dictionary<string, DiaInfo> dictionary = new Dictionary<string, DiaInfo>();

        if (entries != null)
        {
            foreach (DiaInfoEntry entry in entries)
            {
                dictionary[entry.key] = entry.value;
            }
        }

        return dictionary;
    }
}
[Serializable]
public class DataCourse
{
    public string courseName;
    public bool closed = false;
}