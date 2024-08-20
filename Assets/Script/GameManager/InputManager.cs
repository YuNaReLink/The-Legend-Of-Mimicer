using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static float CameraXInput() { return Input.GetAxis("CameraX"); }
    public static float CameraYInput() { return Input.GetAxis("CameraY"); }
    public static bool AttackButton() { return Input.GetButtonDown("Attack"); }
    public static bool AttackHoldButton() { return Input.GetButton("Attack"); }
    public static bool PushMouseRight() { return Input.GetMouseButtonDown(1); }
    public static bool GuardHoldButton() { return Input.GetAxis("Guard") > 0.5f; }
    public static bool PushMouseMiddle() { return Input.GetMouseButtonDown(2); }
    public static bool MenuButton() {  return Input.GetButtonDown("Menu"); }
    public static bool PushESCKey() {  return Input.GetKeyDown(KeyCode.Escape); }
    public static bool ActionButton() {  return Input.GetButtonDown("Action"); }
    public static float HorizontalInput() { return Input.GetAxis("Horizontal"); }
    public static float VerticalInput() { return Input.GetAxis("Vertical"); }
    public static bool UpButton() {  return Input.GetAxis("Vertical") > 0; }
    public static bool DownButton() {  return Input.GetAxis("Vertical") < 0; }
    public static bool LeftButton() {  return Input.GetAxis("Horizontal") > 0; }
    public static bool RightButton() {  return Input.GetAxis("Horizontal") < 0; }
    public static bool LockCameraButton() {  return Input.GetButtonDown("LockCamera"); }
    public static bool GetItemButton() {  return Input.GetButtonDown("Get"); }
    public static bool ChangeButton() {  return Input.GetButtonDown("Change"); }
    public static bool ToolButton() { return Input.GetButtonDown("Tool"); }
}
