using UnityEngine;

public class SauceMaker : MonoBehaviour
{
    private Camera mainCamera;
    public TrailRenderer trailRendererPrefab;
    private GameObject obj;
    public float verticalSpeed = 1.0f;
    private bool isDragging = false;
    private TrailRenderer trailRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
        mainCamera = Camera.main;
        if (GetComponent<BoxCollider2D>() == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
		else
		{
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
		}
        boxCollider.size = new Vector2(Screen.width, Screen.height);
    }

    void OnMouseDown()
    {
        CreateTrailRenderer();

        SetObjectToMousePosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            MoveObject();
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void MoveObject()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        obj.transform.position = new Vector3(worldPosition.x, obj.transform.position.y + (verticalSpeed * Time.deltaTime), transform.position.z);
    }
    void SetObjectToMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        obj.transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    void CreateTrailRenderer()
    {
        if (trailRenderer != null)
        {
            Destroy(trailRenderer.gameObject);
        }

        obj = Instantiate(trailRendererPrefab).gameObject;
        obj.transform.SetParent(gameObject.transform);
    }


}
