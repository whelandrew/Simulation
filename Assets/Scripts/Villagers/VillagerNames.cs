using UnityEngine;

public class VillagerNames
{
    private string first_m = "Alek, Drake, William, Perin, Chris, Ceithen, Daddy, Yan, Sho, Taga";
    private string last_m = "Krominof, Arlin, Dog, Wolf, Boi, Johnson, Bear, Array";

    private char[] delimeters = { ',' };

    public string GetRandomFirstName(int gender)
    {
        string[] names = first_m.Split(delimeters);
        if(gender ==1)
        {
            //female names
        }
        return names[Random.Range(0, names.Length)];        
    }

    public string GetRandomLastName(int gender)
    {
        string[] names = last_m.Split(delimeters);
        if (gender == 1)
        {
            //female names
        }
        return names[Random.Range(0, names.Length)];
    }
}
