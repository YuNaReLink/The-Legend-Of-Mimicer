using UnityEngine;

/// <summary>
/// �L�����N�^�[�̑̂�Renderer���擾���ĐF��ς��鏈�����s���N���X
/// </summary>
public class RendererEffect : MonoBehaviour
{
    private CharacterController     characterController = null;
    public RendererEffect(CharacterController _controller)
    {
        characterController = _controller;
    }

    private Color                   damageColor = Color.red;

    // �F���ς��̂ɂ����鎞��
    private float                   transitionDuration = 1.0f;

    private float                   transitionTimer = 0f;

    private bool                    changeFlag = false;

    public void ColorChange()
    {
        if (!changeFlag) { return; }
        // �w�肳�ꂽ���ԓ��ɐF��ω�������
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        // ���̐F����ڕW�̐F�Ɍ������ĕ��
        for(int i = 0; i < characterController.GetRendererData().RendererList.Count; i++)
        {
            for(int j = 0; j < characterController.GetRendererData().RendererList[i].materials.Length; j++)
            {
                characterController.GetRendererData().RendererList[i].materials[j].color = 
                    Color.Lerp(damageColor, characterController.GetRendererData().GetOriginalColorList()[j],t);
            }
        }

        // ���Ԃ��o�߂����珈�����~
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
