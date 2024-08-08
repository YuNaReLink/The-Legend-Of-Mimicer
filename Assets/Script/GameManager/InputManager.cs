using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static bool PushMouseLeft() { return Input.GetMouseButtonDown(0); }
    public static bool HoldMouseLeft() { return Input.GetMouseButton(0); }
    public static bool PushMouseRight() { return Input.GetMouseButtonDown(1); }
    public static bool HoldMouseRight() { return Input.GetMouseButton(1); }
    public static bool PushMouseMiddle() { return Input.GetMouseButtonDown(2); }
    public static bool PushTabKey() {  return Input.GetKeyDown(KeyCode.Tab); }
    public static bool PushESCKey() {  return Input.GetKeyDown(KeyCode.Escape); }
    public static bool PushShiftKey() {  return Input.GetKeyDown(KeyCode.LeftShift)|| Input.GetKeyDown(KeyCode.RightShift); }
    public static bool PushWKey() {  return Input.GetKeyDown(KeyCode.W); }
    public static bool PushSKey() {  return Input.GetKeyDown(KeyCode.S); }
    public static bool PushAKey() {  return Input.GetKeyDown(KeyCode.A); }
    public static bool PushDKey() {  return Input.GetKeyDown(KeyCode.D); }
    public static bool PushCKey() {  return Input.GetKeyDown(KeyCode.C); }
    public static bool PushFKey() {  return Input.GetKeyDown(KeyCode.F); }
    public static bool PushQKey() {  return Input.GetKeyDown(KeyCode.Q); }
    public static bool PushEKey() { return Input.GetKeyDown(KeyCode.E); }
}
