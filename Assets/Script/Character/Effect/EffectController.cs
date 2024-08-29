using UnityEngine;

/// <summary>
/// EffectController�Ŏg���^�O���܂Ƃ߂�
/// namespace
/// </summary>
namespace EffectTagList
{
    public enum CharacterEffectTag
    {
        Damage,
        Death
    }

    public enum BreakObjectTag
    {
        Break
    }

    public enum SwordHitTag
    {
        Hit
    }
}
/// <summary>
/// �G�t�F�N�g�𐶐�����N���X
/// �G�t�F�N�g�𔭐����������I�u�W�F�N�g�ɃA�^�b�`���Ďg��
/// </summary>
public class EffectController : MonoBehaviour
{
    [SerializeField]
    private VFXScriptableObject vfxObjects = null;

    public void CreateVFX(int number,Vector3 position,float scale,Quaternion quaternion)
    {
        GameObject effect = Instantiate(vfxObjects.GetVFXArray()[number], position, quaternion);
        effect.transform.localScale *= scale;
    }
}
