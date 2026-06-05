using UnityEngine;

public class GruntEnemyStates 
{


    /// IDLE STATE IDLE STATE IDLE STATE IDLE STATEIDLE STATE IDLE STATE
    public class GruntIdleState : IState
    {
        private GruntEnemy grunt;

        public GruntIdleState(GruntEnemy grunt)
        {
            this.grunt = grunt;
        }

        public void Enter()
        {
            grunt.idleWalkTimer = 0;
            Debug.Log(" GRUNT IDLE STATE");
            grunt.StartCoroutine(grunt.idleShuffle());
        }

        public void Exit()
        {

        }

        public void Update()
        {
            //if (grunt.idleWalkTimer > 0) { Debug.Log(grunt.idleWalkTimer); }
        }
    }

    /// IDLE STATE IDLE STATEIDLE STATE IDLE STATEIDLE STATE IDLE STATEIDLE STATE IDLE STATE

}
