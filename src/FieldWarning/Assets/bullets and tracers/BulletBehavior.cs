/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using AssemblyCSharp;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Bullet bullet; //contains attributes for the shell 
    public Transform currentpos;

    //Vector3 LaunchVelocity;
    public float LaunchAngle;
    public float Distance;
    public Vector3 TargetObject;
    private Rigidbody rigid;
    bool Fired;



    //class used in the weapon behaviour to set stats of the shell
    public void SetUp(Transform position,Vector3 target,float launchAngel)
    {
        currentpos = position;
        TargetObject = target;
        LaunchAngle = launchAngel;
    }

    
    //Note blackwolf and math dont mix 

    // Use this for initialization
    void Start()
    {
        bullet = new Bullet();
        rigid = GetComponent<Rigidbody>();
       //currentpos = this.gameObject.transform.position;
        Fired = false;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Positioin is " + transform.position.ToString());

        if (Fired == false)
        {
            Launch();
        }

        // update the rotation of the projectile during trajectory motion
        transform.rotation = Quaternion.LookRotation(rigid.velocity);

    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        Debug.Log("HIT");
        // Play a sound if the colliding objects had a big impact.
        GameObject ImpactedObject = collision.gameObject;


        Destroy(this.gameObject);
    }


    public void Launch()
    {
       
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(TargetObject.x, 0.0f, TargetObject.z);

        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y/500;     //TODO Readjust for our scale usinga utility class
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = TargetObject.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
        Fired = true;

    }

   

    //public void setBullet(Vector3 StartPosition, Vector3 EndPosition, float Vellocity = 30, int arc = 60)
    //{
    //    bullet._startPosition = StartPosition;
    //    bullet._endPosition = EndPosition;
    //    bullet._vellocity = Vellocity;
    //    bullet._arc = 60;
    //}



}
