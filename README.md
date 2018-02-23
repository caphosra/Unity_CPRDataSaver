# Unity_CPRDataSaver
With CPRSaveData, you can create encrypted save data that is hard to be rewritten!

## Description
This library allows you to encrypt and save serializable classes or structures you created.
At the present stage, PlayerPrefs stores encrypted data.
Both will make it possible to output to a file.

## Usage
If you make a class like the following ...
```C#
[System.Serializable]
public class PlayerSaveData
{
    public long money;
    public int score;
    public float speed;
}
```
In this way you can save the data.
```C#
// using CPRUnitySystem;

CPRDataSaver.SetPlayerPrefsEncrypt("Key", playerSaveData, "password!!!!!!!!");
```
You can do this in retrieving data.
```C#
var playerSaveData = CPRDataSaver.SetPlayerPrefsEncrypt<PlayerSaveData>("Key", "password!!!!!!!!");
```
It can also be used without encryption.
```C#
// Save
CPRDataSaver.SetPlayerPrefs("KeyName", playerSaveData);
// Load
playerSaveData = CPRDataSaver.GetPlayerPrefs<PlayerSaveData>("KeyName");
```

## Install
### When to use  
Please go to [ReleasePage](https://github.com/capra314cabra/Unity_CPRDataSaver/releases).
### When to clone this repository  
Please use the following command.  
`git clone https://github.com/capra314cabra/Unity_CPRDataSaver.git`

## Author
[capra314cabra](https://github.com/capra314cabra/)
