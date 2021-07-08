
public class ArrayManager
{
    public bool FindMatchingTData(TData[] arr, TData target)
    {
        for(int i=0;i<arr.Length;i++)
        {
            if(arr[i]==target)
            {
                return true;
            }
        }

        return false;
    }

    public TData[] ResizeTData(TData[] arr)
    {
        int count = 0;
        for(int i=0;i<arr.Length;i++)
        {
            if(arr[i] != null)
            {
                count++;
            }
        }

        TData[] newArr = new TData[count];
        count = 0;
        for(int i=0;i<arr.Length;i++)
        {
            if(arr[i]!=null)
            {
                newArr[count] = arr[i];
                count++;
            }
        }

        return newArr;
    }

    public TData[] RemoveTData(TData[] arr, TData target)
    {
        TData[] newArr = new TData[arr.Length - 1];
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == target || arr[i] == null)
            {
                i++;
            }
            else
            {
                newArr[count] = arr[i];
                count++;
            }
        }

        return newArr;
    }

    public TData[] AddTData(TData[] arr, TData target)
    {
        //if len==1
        int len = arr.Length;
        TData[] newArr = new TData[len+1];
        for(int i=0;i<arr.Length;i++)
        {
            newArr[i] = arr[i];
        }

        if (len == 0)
        {
            newArr[len] = target;
        }
        else
        {
            newArr[newArr.Length-1] = target;
        }
        return newArr;        
    }
}