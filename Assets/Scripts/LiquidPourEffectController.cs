using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidPourEffectController : MonoBehaviour
{
	public static LiquidPourEffectController liquidPourEffectController;
	public LineRenderer[] lineRenderers;
	private Vector3 targetPosition = Vector3.zero;
	public Material SteamMat;
	private Material steamMat;

	private void Awake()
	{
		if(liquidPourEffectController == null)
		{
			LiquidPourEffectController.liquidPourEffectController = this;
		}
		else
		{
			Destroy(this);
		}
	}
	// Start is called before the first frame update
	public void Begin()
	{
		StartCoroutine(BeginPour());
		foreach (var lineRenderer in lineRenderers) lineRenderer.gameObject.SetActive(true);

	}

	public void SetMaterial(Color color)
	{
		steamMat = new Material(SteamMat);
		foreach (var lineRenderer in lineRenderers)
		{
			lineRenderer.sharedMaterial = steamMat;
			steamMat.SetColor("_Color", color);

		}
	}
	public void stopCoffeePouring()
	{
		StopAllCoroutines();
		foreach (var lineRenderer in lineRenderers) lineRenderer.gameObject.SetActive(false);
	}
	private IEnumerator BeginPour()
	{

		while (gameObject.activeSelf)
		{

			targetPosition = FindEndPoint();

			MoveToPosition(0, transform.position);
			MoveToPosition(1, targetPosition);

			yield return null;
		}
	}
	private Vector3 FindEndPoint()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.down);

		Physics.Raycast(ray, out hit, 2.0f);
		Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);
		return endPoint;
	}
	private void MoveToPosition(int index, Vector3 targetPosition)
	{
		foreach (var lineRenderer in lineRenderers) lineRenderer.SetPosition(index, new Vector3(lineRenderer.transform.position.x,targetPosition.y, lineRenderer.transform.position.z));
	}
}
