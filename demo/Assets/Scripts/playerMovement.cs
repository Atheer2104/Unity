using UnityEngine;

public class playerMovement : MonoBehaviour {

    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;

    // Update is called once per frame
    void FixedUpdate() {
      //x, y, z, forcemode Time.deltaTime depends on how good computer you have
      rb.AddForce(0, 0, forwardForce * Time.deltaTime);

      if (Input.GetKey("d")) {
        // force going right
        rb.AddForce(sidewaysForce  * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
      }

      if (Input.GetKey("a")) {
        // force going left
        rb.AddForce(-sidewaysForce  * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
      }

      if (rb.position.y < -1f) {
        FindObjectOfType<GameManager>().EndGame();
      }

    }
}
