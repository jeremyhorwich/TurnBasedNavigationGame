using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_ReverseShoot : MonoBehaviour, IAction
{
    public int Priority => priority;
    [SerializeField] private int priority;

    private Vector3 targetDir { get
        {
            Vector3 _targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (_targetDir == new Vector3(1, 0, 1)) _targetDir = new Vector3(1, 0, 0);      //Protect against diagonal movement
            return _targetDir;
        } }
    private Node target => _target ?? (_target = Gridf.GetFarthestNodeVisible(Gridf.GetNode(transform.position), targetDir, new List<int>()));
    private Node _target; 

    [SerializeField] private Projectile arrow;

    public void InitializeProjectile(GameObject projectileObject)
    {
        Projectile p = projectileObject.GetComponent<Projectile>();
    }

    public bool ActConditionIsMet { get
        {
            if (targetDir == Vector3.zero) return false;        //Should be caching targetDir somewhere or performing this check elsewhere
            return target == Gridf.GetNode(transform.position);
        } }

    public void Act()
    {
        ICommand shootAtTarget = new InstantiateObjectCommand(arrow.gameObject, InitializeProjectile, target.WorldPosition, Quaternion.LookRotation(targetDir));
        CommandManager.instance.SendCommand(shootAtTarget);
    }
}
