using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateSwordTrail : MonoBehaviour
{
    //�@����
    [SerializeField]
    private Transform startPosition;
    //�@����
    [SerializeField]
    private Transform endPosition;
    //�@���b�V��
    private Mesh mesh;
    //�@�O�՗p�̎l�p�`�̕\����
    [SerializeField]
    private int saveMeshNum = 10;
    //�@�ʂ̕�����
    [SerializeField]
    private int faceDivisionNum = 3;
    //�@�O�Ղ̕\���̃I���E�I�t�t���O
    private bool isSwordTrail = false;

    //�@���_���X�g
    [SerializeField]
    private List<Vector3> verticesLists = new List<Vector3>();
    //�@UV���X�g
    [SerializeField]
    private List<Vector2> uvsLists = new List<Vector2>();
    //�@�����̈ʒu���X�g
    [SerializeField]
    private List<Vector3> startPoints = new List<Vector3>();
    //�@����̈ʒu���X�g
    [SerializeField]
    private List<Vector3> endPoints = new List<Vector3>();
    //�@�O�p�`�̃��X�g
    [SerializeField]
    private List<int> tempTriangles = new List<int>();


    // Use this for initialization
    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void LateUpdate()
    {

        //�@�K�v���_���𒴂�����폜
        if (startPoints.Count >= 3 + saveMeshNum)
        {
            startPoints.RemoveAt(0);
            endPoints.RemoveAt(0);
        }
        //�@�����ƌ���̈ʒu��ۑ�
        startPoints.Add(startPosition.position);
        endPoints.Add(endPosition.position);

        //�@���_��4�ȏ�ɂȂ����猕�̋O�Ճ��b�V�����쐬
        if (startPoints.Count >= 3 + saveMeshNum)
        {
            CreateMesh();
        }
    }
    //�@���̋O�Ս쐬���\�b�h
    private void CreateMesh()
    {

        //�@���b�V���A���_�AUV�̃N���A
        mesh.Clear();
        verticesLists.Clear();
        uvsLists.Clear();

        //�@�|�C���g�̊Ԃ̓_�̕ۑ��ϐ�
        Vector3[] startHalf = new Vector3[faceDivisionNum];
        Vector3[] endHalf = new Vector3[faceDivisionNum];

        //�@�|�C���g�Ԃ̈ʒu����
        float addFloatParam = 1f / faceDivisionNum;

        //�@�ʔԍ�
        int i = 0;
        //�@�����ԍ�
        int j = 0;
        //�@UVMap�����ԍ�
        int k = 0;

        for (i = 0; i < saveMeshNum; i++)
        {
            for (j = 0; j < faceDivisionNum - 1; j++)
            {
                //			Debug.Log ("i: " + i + "j: " + j);

                //�@�|�C���g�̍Ō�
                if (i == saveMeshNum - 1)
                {
                    startHalf[j] = Catmull_Rom(startPoints[startPoints.Count - 3], startPoints[startPoints.Count - 2], startPoints[startPoints.Count - 1], (startPoints[startPoints.Count - 3] + startPoints[startPoints.Count - 1]) / 2f, addFloatParam);
                    endHalf[j] = Catmull_Rom(endPoints[endPoints.Count - 3], endPoints[endPoints.Count - 2], endPoints[endPoints.Count - 1], (endPoints[endPoints.Count - 3] + endPoints[endPoints.Count - 1]) / 2f, addFloatParam);
                    //�@�|�C���g�ƃ|�C���g�̊�
                }
                else
                {
                    startHalf[j] = Catmull_Rom(startPoints[startPoints.Count - (saveMeshNum - i) - 2], startPoints[startPoints.Count - (saveMeshNum - i) - 1], startPoints[startPoints.Count - (saveMeshNum - i)], startPoints[startPoints.Count - (saveMeshNum - i) + 1], addFloatParam);
                    endHalf[j] = Catmull_Rom(endPoints[endPoints.Count - (saveMeshNum - i) - 2], endPoints[endPoints.Count - (saveMeshNum - i) - 1], endPoints[endPoints.Count - (saveMeshNum - i)], endPoints[endPoints.Count - (saveMeshNum - i) + 1], addFloatParam);
                }
                //�@�����̊������v�Z
                addFloatParam += 1f / faceDivisionNum;
            }
            //�@�ŏ��̖ʂ̎��͍ŏ��̂Q�_��ǉ�	
            if (i == 0)
            {
                verticesLists.AddRange(new Vector3[] {
                startPoints [startPoints.Count - (saveMeshNum - i) - 1],
                endPoints [endPoints.Count - (saveMeshNum - i) - 1]
            });
                Debug.DrawLine(startPoints[startPoints.Count - (saveMeshNum - i) - 1], endPoints[endPoints.Count - (saveMeshNum - i) - 1], Color.red);
            }
            //�@�|�C���g�ƊԂ̓_����O�p�`���쐬
            for (k = 0; k < faceDivisionNum - 1; k++)
            {
                verticesLists.AddRange(new Vector3[] {
                startHalf [k],
                endHalf [k]
            });
                //�@�������m�F
                if (i == 0)
                {
                    Debug.DrawLine(startHalf[k], endHalf[k], Color.yellow);
                }
                else if (i == 1)
                {
                    Debug.DrawLine(startHalf[k], endHalf[k], Color.white);
                }
                else if (i == 2)
                {
                    Debug.DrawLine(startHalf[k], endHalf[k], Color.blue);
                }
            }
            //�@�Ō�̂Q�_��ǉ�
            verticesLists.AddRange(new Vector3[] {
            startPoints [startPoints.Count - (saveMeshNum - i)],
            endPoints [endPoints.Count - (saveMeshNum - i)]
        });

            Debug.DrawLine(startPoints[startPoints.Count - (saveMeshNum - i)], endPoints[endPoints.Count - (saveMeshNum - i)], Color.green);


            addFloatParam = 1f / faceDivisionNum;
        }

        addFloatParam = 0f;

        //�@UVMAP�p�̒��_�̍쐬
        float addParam = 0f;
        for (i = 0; i < verticesLists.Count; i++)
        {
            if (i % 2 == 0)
            {
                uvsLists.Add(new Vector2(addParam / (saveMeshNum * faceDivisionNum), 0f));
            }
            else
            {
                uvsLists.Add(new Vector2(addParam / (saveMeshNum * faceDivisionNum), 1f));
                addParam++;
            }
        }

        //�@���_���烁�b�V���̎O�p�`�p�̒��_�ԍ��w��
        List<int> tempTriangles = new List<int>();
        for (i = 0, j = 0; j < saveMeshNum * faceDivisionNum; i += 2, j++)
        {
            tempTriangles.AddRange(new int[] {
            i, i + 1, i + 2,
            i + 2, i + 1, i + 3
        });
        }

        if (isSwordTrail)
        {
            mesh.vertices = verticesLists.ToArray();
            mesh.uv = uvsLists.ToArray();
            mesh.triangles = tempTriangles.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
    //�@�Â����̋O�Ճ��b�V���̍폜
    private void DeleteMesh()
    {
        verticesLists.RemoveRange(0, faceDivisionNum * 1);
        uvsLists.RemoveRange(0, faceDivisionNum * 1);
    }
    //�@�Ȑ������ׂ̒��_�̌v�Z���\�b�h
    Vector3 Catmull_Rom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * ((2 * p1)
            + (-p0 + p2) * t
            + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t
            + (-p0 + 3 * p1 - 3 * p2 + p3) * t * t * t);
    }

    public void SetSwordTrail(bool swordTrailFlag)
    {
        isSwordTrail = swordTrailFlag;
    }
}
