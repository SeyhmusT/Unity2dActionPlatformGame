using UnityEngine;

public class TestSaveLoad : MonoBehaviour
{
    public ExampleData someData;
    private void Start()
    {
        SaveLoadManager.instance.DeleteExample("test.json");
        SaveLoadManager.instance.LoadExample(someData, "test.json");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            someData.exampleInt = 5;
            someData.exampleString = "Example";
            SaveLoadManager.instance.SaveExample(someData, "test.json");
        }
    }
}
