# MinMaxSlider
![Vector2 Example](Readme/AllSlidersReference.png)

Unity Package to easily create a MinMaxSlider for your needs. Get started within seconds and configure your ranged values nicely and error safe.
 - Works with prefabs and overriden properties
 - Reposition and hide fields
 - Support for lots of types
 - Use quickly with custom attribute
 - Use in custom inspector with editor code
 
## How to install
This is a Unity package and can be installed via the package manager.
 1. go to **Window -> Package Manager**
 2. click on the **+** in the top left (or bottom right in older Unity versions)
 3. select "**Add package from git URL...**"
 4. paste the git url to this project

For a more detailed guide on how to install packages check out the official [Package Manager site](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#:~:text=Use%20the%20Unity%20Package%20Manager,update%20packages%20for%20each%20project.).

## Supported Types
 - Vector2
 - Vector2Int
 - Float, Double
 - Integer, Long

## How to use: Attribute
Use this namespace to use the attribute:
```CSharp
using Zelude;
```

### Vector2, Vector2Int
![Vector2 Example](Readme/Vector2.png)

### Float, Double, Integer, Long
![Vector2 Example](Readme/Float.png)

## How to use: Custom Editor
Use this namespace to use the editor code:
```CSharp
using ZeludeEditor;
```
All the Editor Code you need is in **MMSEditorGUI** and can be called like this:
```CSharp
MMSEditorGUI.MinMaxSlider(position, EditorGUIUtility.TrTempContent("My Slider"), myVector, 0f, 1f, SliderFieldPosition.Left, SliderFieldPosition.Right);
```
