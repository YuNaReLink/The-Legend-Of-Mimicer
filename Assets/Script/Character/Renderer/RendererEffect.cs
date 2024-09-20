using UnityEngine;

/// <summary>
/// キャラクターの体のRendererを取得して色を変える処理を行うクラス
/// </summary>
public class RendererEffect : MonoBehaviour
{
    private CharacterController     characterController = null;
    public RendererEffect(CharacterController _controller)
    {
        characterController = _controller;
    }

    private Color                   damageColor = Color.red;

    // 色が変わるのにかかる時間
    private float                   transitionDuration = 1.0f;

    private float                   transitionTimer = 0f;

    private bool                    changeFlag = false;

    public void ColorChange()
    {
        if (!changeFlag) { return; }
        // 指定された時間内に色を変化させる
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        // 元の色から目標の色に向かって補間
        for(int i = 0; i < characterController.GetRendererData().RendererList.Count; i++)
        {
            for(int j = 0; j < characterController.GetRendererData().RendererList[i].materials.Length; j++)
            {
                characterController.GetRendererData().RendererList[i].materials[j].color = 
                    Color.Lerp(damageColor, characterController.GetRendererData().GetOriginalColorList()[j],t);
            }
        }

        // 時間が経過したら処理を停止
        if (transitionTimer >= transitionDuration)
        {
            changeFlag = false;
        }
    }

    public void SetChangeFlag()
    {
        changeFlag = true;
        transitionTimer = 0f;
    }
}
