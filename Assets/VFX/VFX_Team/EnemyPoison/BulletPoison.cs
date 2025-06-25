using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakanVFX
{
    public class BulletPoison : MonoBehaviour
    {
        public GameObject vfx_DestroyPoison;
        public float lifetime;
        public float fieldpoisonLife;
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, lifetime);

        }

        private void DestroyActive()
        {

            Destroy(Instantiate(vfx_DestroyPoison, transform.position, Quaternion.identity), fieldpoisonLife);

        }
        private void OnDestroy()
        {
            DestroyActive();
        }
    }
}
