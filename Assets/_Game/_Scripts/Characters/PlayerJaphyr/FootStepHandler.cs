using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    [SerializeField] GameObject footstepParticlePrefab; // Prefab of the particle system
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;
    [SerializeField] int groundLayer;
    private float raycastSize = 10f;

    private void Update()
    {
        Debug.DrawRay(leftFoot.position, Vector3.down * raycastSize, Color.green);
        Debug.DrawRay(rightFoot.position, Vector3.down * raycastSize, Color.green);
    }

    // Called by animation event
    public void OnFootstepLeft()
    {
        HandleFootstepParticles(leftFoot);
    }

    // Called by animation event
    public void OnFootstepRight()
    {
        HandleFootstepParticles(rightFoot);
    }

    private void HandleFootstepParticles(Transform foot)
    {
        RaycastHit hit;
        Ray ray = new Ray(foot.position, Vector3.down);
        int layerMask = 1 << groundLayer;

        if (Physics.Raycast(ray, out hit, raycastSize, layerMask))
        {
            // Instantiate the particle system at the hit point
            GameObject particleObj = Instantiate(footstepParticlePrefab, hit.point, Quaternion.identity);
            ParticleSystem particle = particleObj.GetComponent<ParticleSystem>();
            particle.Simulate(2f, true, true);
            particle.Play();
            Destroy(particleObj, particle.main.duration * 2.5f);
        }
    }
}
