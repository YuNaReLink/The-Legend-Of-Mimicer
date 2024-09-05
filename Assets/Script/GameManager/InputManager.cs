using UnityEngine;


/// <summary>
/// ゲーム中に使用するボタン入力をまとめたクラス
/// Axis名はProject Setting内のInputManagerを参照
/// </summary>
public class InputManager
{
    public static float CameraXInput() { return Input.GetAxis("CameraX"); }
    public static float CameraYInput() { return Input.GetAxis("CameraY"); }
    public static bool AttackButton() { return Input.GetButtonDown("Attack"); }
    public static bool AttackHoldButton() { return Input.GetButton("Attack"); }
    public static bool PushMouseRight() { return Input.GetMouseButtonDown(1); }
    public static bool GuardHoldButton() { return Input.GetAxis("Guard") > 0.5f; }
    private static bool guardPush = false;
    public static bool GuardPushButton()
    {
        if(Input.GetAxis("Guard") > 0)
        {
            if (guardPush) { return false; }
            guardPush = true;
            return Input.GetAxis("Guard") > 0;
        }
        else
        {
            guardPush = false;
            return false;
        }
    }
    public static bool PushMouseMiddle() { return Input.GetMouseButtonDown(2); }
    public static bool MenuButton() {  return Input.GetButtonDown("Menu"); }
    public static bool PushESCKey() {  return Input.GetKeyDown(KeyCode.Escape); }
    public static bool ActionButton() {  return Input.GetButtonDown("Action"); }
    public static float HorizontalInput() { return Input.GetAxis("Horizontal"); }
    public static float VerticalInput() { return Input.GetAxis("Vertical"); }

    private static bool upFlag = false;
    public static bool UpButton() 
    {
        if (Input.GetAxisRaw("MenuButtonY") > 0)
        {
            if (upFlag) { return false; }
            upFlag = true;
            return Input.GetAxisRaw("MenuButtonY") > 0;
        }
        else
        {
            upFlag = false;
            return false;
        }
    }
    private static bool downFlag = false;
    public static bool DownButton() 
    {
        if (Input.GetAxisRaw("MenuButtonY") < 0)
        {
            if (downFlag) { return false; }
            downFlag = true;
            return Input.GetAxisRaw("MenuButtonY") < 0;
        }
        else
        {
            downFlag = false;
            return false;
        }
    }
    private static bool leftFlag = false;
    public static bool LeftButton() 
    {
        if (Input.GetAxisRaw("MenuButtonX") < 0)
        {
            if (leftFlag) { return false; }
            leftFlag = true;
            return Input.GetAxisRaw("MenuButtonX") < 0;
        }
        else
        {
            leftFlag = false;
            return false;
        }
    }
    private static bool rightFlag = false;
    public static bool RightButton() 
    {
        if (Input.GetAxisRaw("MenuButtonX") > 0)
        {
            if (rightFlag) { return false; }
            rightFlag = true;
            return Input.GetAxisRaw("MenuButtonX") > 0;
        }
        else
        {
            rightFlag = false;
            return false;
        }
    }
    public static bool LockCameraButton() {  return Input.GetButtonDown("LockCamera"); }
    public static bool GetItemButton() {  return Input.GetButtonDown("Get"); }
    public static bool ChangeButton() {  return Input.GetButtonDown("Change"); }
    public static bool ToolButton() { return Input.GetButtonDown("Tool"); }

    public enum DeviceInput
    {
        Key,
        Controller
    }

    private static DeviceInput deviceInput = DeviceInput.Key;
    public static DeviceInput GetDeviceInput() { return deviceInput; }

    public static void CheckInput()
    {
        if (Input.anyKey)
        {
            deviceInput = DeviceInput.Key;
        }

        // コントローラーボタンのチェック
        for (int i = 0; i < 14; i++)
        {
            if (Input.GetKey("joystick button" + " " + i))
            {
                deviceInput = DeviceInput.Controller;
                break;
            }
        }

        // コントローラーの軸のチェック
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&
           !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            if ((Mathf.Abs(Input.GetAxis("Horizontal")) >= 1.0f) ||
                (Mathf.Abs(Input.GetAxis("Vertical")) >= 1.0f))
            {
                deviceInput = DeviceInput.Controller;
            }
        }
    }

}
