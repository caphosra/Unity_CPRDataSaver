using UnityEngine;
using CPRUnitySystem;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var t = new TestClass();
        t.a = 3;
        t.b = 5;
        Debug.Log("TestClass : a=" + t.a + " b=" + t.b);

        Debug.Log("Save and load");
        CPRDataSaver.SetPlayerPrefs("Test", t);
        t = CPRDataSaver.GetPlayerPrefs<TestClass>("Test");
        Debug.Log("TestClass : a=" + t.a + " b=" + t.b);

        Debug.Log("Save and load");
        PlayerPrefs.SetString("TestEnc", "");
        CPRDataSaver.LogSecurity = LogSecurity.Low;
        CPRDataSaver.SetPlayerPrefsEncrypt("TestEnc", t, "password");
        t = CPRDataSaver.GetPlayerPrefsEncrypt<TestClass>("TestEnc", "password");
        Debug.Log("TestClass : a=" + t.a + " b=" + t.b);

        Debug.Log("Save and load");
        CPRDataSaver.LogSecurity = LogSecurity.Low;
        CPRDataSaver.SetDataFileEncrypt(Application.dataPath + "/TestEnc.enc", t, "password");
        t = CPRDataSaver.GetDataFileEncrypt<TestClass>(Application.dataPath + "/TestEnc.enc", "password");
        Debug.Log("TestClass : a=" + t.a + " b=" + t.b);
    }
}

[System.Serializable]
public class TestClass
{
    public int a;
    public int b;
}
