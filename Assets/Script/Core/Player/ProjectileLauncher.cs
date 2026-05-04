using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;

    private bool shouldFire;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) {return;};
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
        
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) {return;};
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
        
    }
    void Update()
    {
        if(!IsOwner) {return;}
        if(!shouldFire) {return;}

        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);

        SpawnDummtProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(
            serverProjectilePrefab,
            spawnPos,
            Quaternion.identity);

            projectileInstance.transform.up = direction;
            SpawnDummtProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummtProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if(IsOwner) {return;}

        SpawnDummtProjectile(spawnPos, direction);
    }
    private void SpawnDummtProjectile(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(
            clientProjectilePrefab,
            spawnPos,
            Quaternion.identity);

        projectileInstance.transform.up = direction;
    }
}
