// See https://aka.ms/new-console-template for more information

using System.Collections;
using ClassLibrary2;

{
    Console.WriteLine("[Main] LoadAsync test begin");
    LoadManager.Instance.SetRootPath(@"C:\Users\MECHREVO\Desktop");
    LoadStringAsync("NetCore.txt");

    int framecount = 0;  //模拟当前帧数,当前time下,1秒50帧
    while (true)
    {
        framecount++;
        Console.WriteLine($"[Main] cur framecount:<{framecount}> CoroutineManager.Instance.UpdateCoroutine");

        
        //break;
        
        //模拟untity的update
        if (CoroutineManager.Instance.UpdateCoroutine())
        {
            Thread.Sleep(Time.deltaMilTime);
            if (framecount >= 10)
            {
                break;
            }
        }
        else
        {
            break;
        }
    }
}

static void LoadStringAsync(string fileName)
{
    CoroutineManager.Instance.Start(LoadString(fileName));
}

static IEnumerator LoadString(string fileName)
{
    var request = LoadManager.Instance.LoadText(fileName);
    yield return request;

    if (request != null)
    {
        if (request.Result != null)
        {
            Console.WriteLine("[LoadString] LoadFinished. and str: \n" + request.Result.ToString());
        }
        if(request.Error != null)
            Console.WriteLine(request.Error);
    }
}