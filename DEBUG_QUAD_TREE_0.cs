using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_QUAD_TREE_0 : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(STIMULATE());
        }
        //
    }


    public int[] _rect = new int[4] { 10, 10, 40, 30 };
    public QUAD_NODE _node;


    IEnumerator STIMULATE()
    {

        #region frame_rate

        //
        QualitySettings.vSyncCount = 4;
        yield return null;
        yield return null;
        //
        #endregion



        DRAW.col = Color.red;
        DRAW.dt = 20f;




        DRAW._rect(new Vector2(0, 0), new Vector2(3, 4), 0.1f);




        //int[] _rect = new int[4] { 20, 20, 50, 50 }; 
        int[] _rect = this._rect;

        QUAD_NODE _node = new QUAD_NODE()
        {
            _rect = new int[4] { 0, 0, 255, 255 },
            _depth = 10
        };
        _node.split(_rect);

        this._node = _node;


        DRAW._QUAD_NODE(_node);


        DRAW.col = Color.HSVToRGB(0f, 0.5f, 0.8f);
        DRAW._rect(_rect);





        yield return null;


        /*
        Debug.Log(

            QUAD_NODE._rect_A_B
            (
                new int[4] { 0 , 0 , 10,  10},
                new int[4] { 3 , 3 , 6,  6}

            ));
        */

    }


  


    [System.Serializable]
    public class QUAD_NODE
    {

        public QUAD_NODE[] Q_1D;


        // [ xm , ym , xM , yM ]
        public int[] _rect;

        public int _depth;



        /*
          0 out
          1 in
          2 partial

        */
        public int _state = -1;


        public void split(int[] _rect)
        {
            if(_depth == 0)
            {
                int _state = _rect_A_B(_rect, this._rect);
                if (_state == 0)
                {
                    this._state = 0;
                    //
                }
                else if (_state == 1)
                {
                    this._state = 1;
                    //
                }


                return;
            }
            else
            {
                int _state = _rect_A_B(_rect, this._rect);

                if(_state == 0)
                {
                    this._state = 0;
                    //
                }
                else if(_state == 1)
                {
                    this._state = 1;
                    //
                }
                else
                {
                    //Debug.Log("BOOM..");
                    this._state = 2;
                    //

                    Q_1D = new QUAD_NODE[4]
                    {
                        new QUAD_NODE()
                        {
                            _rect = new int[4]
                            {
                                this._rect[0],
                                this._rect[1],
                                this._rect[0] + (this._rect[2] - this._rect[0]) / 2,
                                this._rect[1] + (this._rect[3] - this._rect[1]) / 2,
                            },
                            _depth = this._depth - 1
                        },

                        new QUAD_NODE()
                        {
                            _rect = new int[4]
                            {
                                this._rect[0] + (this._rect[2] - this._rect[0]) / 2 + 1,
                                this._rect[1],
                                this._rect[2],
                                this._rect[1] + (this._rect[3] - this._rect[1]) / 2,
                            },
                            _depth = this._depth - 1
                        },

                        new QUAD_NODE()
                        {
                            _rect = new int[4]
                            {
                                this._rect[0],
                                this._rect[1] + (this._rect[3] - this._rect[1]) / 2 + 1,
                                this._rect[0] + (this._rect[2] - this._rect[0]) / 2,
                                this._rect[3],
                            },
                            _depth = this._depth - 1
                        },

                        new QUAD_NODE()
                        {
                            _rect = new int[4]
                            {
                                this._rect[0] + (this._rect[2] - this._rect[0]) / 2 + 1,
                                this._rect[1] + (this._rect[3] - this._rect[1]) / 2 + 1,
                                this._rect[2],
                                this._rect[3],
                            },
                            _depth = this._depth - 1
                        },


                    };

                    for(int i  = 0; i < Q_1D .Length; i += 1)
                    {
                        Q_1D[i].split(_rect);
                    }

                }

            }

            //
        }
        //




        /*
        0 - none
        1 - full
        2 - partial 

        */
        public static int _rect_A_B(int[] _rect_A_ , int[] _rect_B_)
        {


            #region 0 - none
            if (_rect_A_[0] > _rect_B_[2]) { return 0; }

            if (_rect_A_[1] > _rect_B_[3]) { return 0; }

            if (_rect_A_[0] < _rect_B_[0])
                if (_rect_A_[2] < _rect_B_[0]) { return 0; }

            if (_rect_A_[1] < _rect_B_[1])
                if (_rect_A_[3] < _rect_B_[1]) { return 0; }
            #endregion




            /*

            QUAD_NODE._rect_A_B
            (
                new int[4] { 0 , 0 , 10,  10},
                new int[4] { 3 , 3 , 6,  6}

            ));

            */


            #region 1 - full

            if (_rect_A_[0] <= _rect_B_[0] &&
               _rect_A_[1] <= _rect_B_[1] &&
               _rect_A_[2] >= _rect_B_[2] &&
               _rect_A_[3] >= _rect_B_[3])
            {
                return 1;
            }

            #endregion





            #region 2 - partial

            if ( _rect_A_[0] <= _rect_B_[2] &&
                 _rect_A_[1] <= _rect_B_[3]
            )
            {
                if (_rect_A_[2] >= _rect_B_[0] &&
                    _rect_A_[3] >= _rect_B_[1]
                )
                {
                    return 2;
                }

            }

            #endregion



            return -1;

        }
        //




    }



    public static class DRAW
    {
        public static float dt;
        public static Color col;



        #region _rect
        // _rect //
        public static void _rect(Vector2 a, Vector2 b, float layer = 0f, float e = 1f / 50, float de = 1f / 20)
        {


            Vector3[] _corner_1D = new Vector3[4]
            {
                new Vector3(a.x + e , a.y + e , -layer),
                new Vector3(a.x + e , b.y - e , -layer),
                new Vector3(b.x - e , b.y - e , -layer),
                new Vector3(b.x - e , a.y + e , -layer),
            };

            Vector3[] _corner_1D_e = new Vector3[4]
            {
                new Vector3(a.x + (e + de), a.y + (e + de) , -layer),
                new Vector3(a.x + (e + de), b.y - (e + de) , -layer),
                new Vector3(b.x - (e + de), b.y - (e + de) , -layer),
                new Vector3(b.x - (e + de), a.y + (e + de) , -layer),
            };


            for (int i = 0; i < _corner_1D.Length; i += 1)
            {
                Debug.DrawLine(_corner_1D[i], _corner_1D[(i + 1) % _corner_1D.Length], col, dt);
            }

            for (int i = 0; i < _corner_1D_e.Length; i += 1)
            {
                Debug.DrawLine(_corner_1D_e[i], _corner_1D_e[(i + 1) % _corner_1D_e.Length], col, dt);
            }


        }

        public static void _rect(int[] _rect_xm_ym_xM_yM, float layer = 0f, float e = 1f/50, float de = 1f / 20)
        {

            _rect(
                new Vector2(_rect_xm_ym_xM_yM[0], _rect_xm_ym_xM_yM[1]),
                new Vector2(_rect_xm_ym_xM_yM[2], _rect_xm_ym_xM_yM[3]),
                layer, e, de
                );

        }
        // _rect // 
        #endregion


        public static void _QUAD_NODE(QUAD_NODE _node)
        {

            if(_node._state == 0 || _node._state == 1)
            {

                     if(_node._state == 0) { DRAW.col = Color.HSVToRGB(0f, 0f, 0.4f); }
                else if(_node._state == 1) { DRAW.col = Color.HSVToRGB(0f, 0f, 0.8f); }


                float e = 0.5f;
                _rect(
                    new Vector2(_node._rect[0] - e,_node._rect[1] - e),
                    new Vector2(_node._rect[2] + e,_node._rect[3] + e),
                layer: 0f);

                return;
            }
            else
            {
                //
                for (int i = 0; i < _node.Q_1D.Length; i += 1)
                {
                    _QUAD_NODE(_node.Q_1D[i]);
                }

                    
            }
            
        }
        //


    }





}
