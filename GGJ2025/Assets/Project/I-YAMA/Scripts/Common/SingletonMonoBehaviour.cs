//  SingletonMonoBehaviour.cs
using UnityEngine;

namespace GGJ.Common
{
    /// <summary>
    /// MonoBehaviourを継承し、初期化メソッドを備えたシングルトンなクラス
    /// </summary>
    public class SingletonMonoBehaviour<T> : MonoBehaviourWithInit where T : MonoBehaviourWithInit
    {
        //インスタンス
        private static T instance;

        //インスタンスを外部から参照する用(getter)
        public static T Instance
        {
            get
            {
                //インスタンスがまだ作られていない
                if (instance == null)
                {
                    //シーン内からインスタンスを取得
                    instance = (T)FindObjectOfType(typeof(T));

                    //シーン内に存在しない場合はエラー
                    if (instance == null)
                    {
                        Debug.LogError(typeof(T) + " is nothing");
                    }
                    //発見した場合は初期化
                    else
                    {
                        instance.InitIfNeeded();
                    }
                }

                return instance;
            }
        }

        //=================================================================================
        //初期化
        //=================================================================================
        protected sealed override void Awake()
        {
            //存在しているインスタンスが自分であれば問題なし
            if (this == Instance)
            {
                return;
            }

            //自分じゃない場合は重複して存在しているので、エラー
            Debug.LogError(typeof(T) + " is duplicated");
        }
    }

    /// <summary>
    /// 初期化メソッドを備えたMonoBehaviour
    /// </summary>
    public class MonoBehaviourWithInit : MonoBehaviour
    {
        //初期化したかどうかのフラグ(一度しか初期化が走らないようにするため)
        private bool isInitialized = false;

        /// <summary>
        /// 必要なら初期化する
        /// </summary>
        public void InitIfNeeded()
        {
            if (isInitialized)
            {
                return;
            }

            Init();
            isInitialized = true;
        }

        /// <summary>
        /// 初期化(Awake時かその前の初アクセス、どちらかの一度しか行われない)
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// sealed overrideするためにvirtualで作成
        /// </summary>
        protected virtual void Awake()
        {
        }
    }
}