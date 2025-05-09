using UnityEngine;

public class Tool : MonoBehaviour {
    public ToolItem tool;
    public GameObject particle;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Use();
    }

    public void Use() {
        RaycastHit hit;

        Transform cameraPivot = Camera.main.transform.parent;

        if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out hit, tool.range)) {
            if (hit.transform.tag != "Material") return;
            
            MaterialObj material = hit.transform.GetComponent<MaterialObj>();

            if (material.material.subItemType != tool.toolType) return;

            material.Mine(tool.damage);

            tool.durability -= 1;

            ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule particleSystemMain = particleSystem.main;
            particleSystemMain.startColor = material.particleColor;

            Instantiate(particle, hit.point, Quaternion.identity);

            if (tool.durability <= 0) Player.inventory.RemoveItem(tool, 1);
        }
    }
}
