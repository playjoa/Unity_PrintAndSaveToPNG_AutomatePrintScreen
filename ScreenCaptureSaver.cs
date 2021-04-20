using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureSaver : MonoBehaviour
{
    [SerializeField]
    private string directoryLocation = "C:/Users/yourUser/Desktop/", folderName = "CarPhotos";

    [SerializeField]
    private string prefixFileName = "img_"; 

    [SerializeField]
    private float shutterTime = 0.1f;

    const string typeOfFile = ".png";

    private void Start()
    {
        StartCoroutine(PrintEachGameObject(GetChildsInObject()));
    }

    IEnumerator PrintEachGameObject(GameObject[] objectsToPrint) 
    {
        DeactivateObjects(objectsToPrint);

        for (int i = 0; i < objectsToPrint.Length; i++)
        {
            objectsToPrint[i].SetActive(true);

            yield return new WaitForSeconds(shutterTime);

            byte[] printBytes = CreateTextureFromGameView().EncodeToPNG();

            CreateFileOnLocation(printBytes);
            
            objectsToPrint[i].SetActive(false);
        }
    }

    GameObject[] GetChildsInObject()
    {
        GameObject[] childs = new GameObject[transform.childCount];

        for (int i = 0; i < childs.Length; i++)
            childs[i] = transform.GetChild(i).gameObject;

        return childs;
    }

    void DeactivateObjects(GameObject[] objectsToDeactivate)
    {
        foreach (GameObject object in objects)
        {
            if(object.activeSelf)
                object.SetActive(false);
        }
    }

    string FolderLocation()
    {
        return directoryLocation + folderName;
    }

    string FileName(int numberFile)
    {
        return prefixFileName + numberFile + typeOfFile;
    }

    Texture2D CreateTextureFromGameView()
    {
        Texture2D tempTexture = new Texture2D(Screen.width, Screen.height);
        tempTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tempTexture.Apply();

        return tex;
    }

    void CreateFileOnLocation(byte printBytes)
    {
        CheckIfLocationExists_CreateIfNecessary();
            
        var path = System.IO.Path.Combine(FolderLocation(), FileName(i));
        System.IO.File.WriteAllBytes(path, bytes);
    }

    void CheckIfLocationExists_CreateIfNecessary()
    {
        if (!System.IO.Directory.Exists(FolderLocation()));
            System.IO.Directory.CreateDirectory(FolderLocation());
    }  
}
