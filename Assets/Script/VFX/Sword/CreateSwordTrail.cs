using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateSwordTrail : MonoBehaviour
{
    //　剣元
    [SerializeField]
    private Transform startPosition;
    //　剣先
    [SerializeField]
    private Transform endPosition;
    //　メッシュ
    private Mesh mesh;
    //　軌跡用の四角形の表示個数
    [SerializeField]
    private int saveMeshNum = 10;
    //　面の分割数
    [SerializeField]
    private int faceDivisionNum = 3;
    //　軌跡の表示のオン・オフフラグ
    private bool isSwordTrail = false;

    //　頂点リスト
    [SerializeField]
    private List<Vector3> verticesLists = new List<Vector3>();
    //　UVリスト
    [SerializeField]
    private List<Vector2> uvsLists = new List<Vector2>();
    //　剣元の位置リスト
    [SerializeField]
    private List<Vector3> startPoints = new List<Vector3>();
    //　剣先の位置リスト
    [SerializeField]
    private List<Vector3> endPoints = new List<Vector3>();
    //　三角形のリスト
    [SerializeField]
    private List<int> tempTriangles = new List<int>();


    // Use this for initialization
    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void LateUpdate()
    {

        //　必要頂点数を超えたら削除
        if (startPoints.Count >= 3 + saveMeshNum)
        {
            startPoints.RemoveAt(0);
            endPoints.RemoveAt(0);
        }
        //　根元と剣先の位置を保存
        startPoints.Add(startPosition.position);
        endPoints.Add(endPosition.position);

        //　頂点が4以上になったら剣の軌跡メッシュを作成
        if (startPoints.Count >= 3 + saveMeshNum)
        {
            CreateMesh();
        }
    }
    //　剣の軌跡作成メソッド
    private void CreateMesh()
    {

        //　メッシュ、頂点、UVのクリア
        mesh.Clear();
        verticesLists.Clear();
        uvsLists.Clear();

        //　ポイントの間の点の保存変数
        Vector3[] startHalf = new Vector3[faceDivisionNum];
        Vector3[] endHalf = new Vector3[faceDivisionNum];

        //　ポイント間の位置割合
        float addFloatParam = 1f / faceDivisionNum;

        //　面番号
        int i = 0;
        //　分割番号
        int j = 0;
        //　UVMap分割番号
        int k = 0;

        for (i = 0; i < saveMeshNum; i++)
        {
            for (j = 0; j < faceDivisionNum - 1; j++)
            {
                //			Debug.Log ("i: " + i + "j: " + j);

                //　ポイントの最後
                if (i == saveMeshNum - 1)
                {
                    startHalf[j] = Catmull_Rom(startPoints[startPoints.Count - 3], startPoints[startPoints.Count - 2], startPoints[startPoints.Count - 1], (startPoints[startPoints.Count - 3] + startPoints[startPoints.Count - 1]) / 2f, addFloatParam);
                    endHalf[j] = Catmull_Rom(endPoints[endPoints.Count - 3], endPoints[endPoints.Count - 2], endPoints[endPoints.Count - 1], (endPoints[endPoints.Count - 3] + endPoints[endPoints.Count - 1]) / 2f, addFloatParam);
                    //　ポイントとポイントの間
                }
                else
                {
                    startHalf[j] = Catmull_Rom(startPoints[startPoints.Count - (saveMeshNum - i) - 2], startPoints[startPoints.Count - (saveMeshNum - i) - 1], startPoints[startPoints.Count - (saveMeshNum - i)], startPoints[startPoints.Count - (saveMeshNum - i) + 1], addFloatParam);
                    endHalf[j] = Catmull_Rom(endPoints[endPoints.Count - (saveMeshNum - i) - 2], endPoints[endPoints.Count - (saveMeshNum - i) - 1], endPoints[endPoints.Count - (saveMeshNum - i)], endPoints[endPoints.Count - (saveMeshNum - i) + 1], addFloatParam);
                }
                //　分割の割合を計算
                addFloatParam += 1f / faceDivisionNum;
            }
            //　最初の面の時は最初の２点を追加	
            if (i == 0)
            {
                verticesLists.AddRange(new Vector3[] {
                startPoints [startPoints.Count - (saveMeshNum - i) - 1],
                endPoints [endPoints.Count - (saveMeshNum - i) - 1]
            });
                Debug.DrawLine(startPoints[startPoints.Count - (saveMeshNum - i) - 1], endPoints[endPoints.Count - (saveMeshNum - i) - 1], Color.red);
            }
            //　ポイントと間の点から三角形を作成
            for (k = 0; k < faceDivisionNum - 1; k++)
            {
                verticesLists.AddRange(new Vector3[] {
                startHalf [k],
                endHalf [k]
            });
                //　分割線確認
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
            //　最後の２点を追加
            verticesLists.AddRange(new Vector3[] {
            startPoints [startPoints.Count - (saveMeshNum - i)],
            endPoints [endPoints.Count - (saveMeshNum - i)]
        });

            Debug.DrawLine(startPoints[startPoints.Count - (saveMeshNum - i)], endPoints[endPoints.Count - (saveMeshNum - i)], Color.green);


            addFloatParam = 1f / faceDivisionNum;
        }

        addFloatParam = 0f;

        //　UVMAP用の頂点の作成
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

        //　頂点からメッシュの三角形用の頂点番号指定
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
    //　古い剣の軌跡メッシュの削除
    private void DeleteMesh()
    {
        verticesLists.RemoveRange(0, faceDivisionNum * 1);
        uvsLists.RemoveRange(0, faceDivisionNum * 1);
    }
    //　曲線を作る為の頂点の計算メソッド
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
