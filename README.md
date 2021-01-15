# MinMaxSlider
![Vector2 Example](Readme/AllSlidersReference.png)

Easily create a MinMaxSlider for your needs. Get started within seconds and configure your ranged values nicely and error safe.
 - Works with prefabs and overriden properties
 - Reposition and hide fields
 - Support for lots of types
 - Use quickly with custom attribute
 - Use in custom inspector with editor code

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
