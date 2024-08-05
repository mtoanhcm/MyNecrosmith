using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFuncion : MonoBehaviour
{
    [SerializeField]
    private int searchAngle;
    [SerializeField]
    private float moveRadius;

    [Button]
    private void Test() {
        transform.position = GetRandomPositionInDirection(GetNewRandomDirection());
    }

    private Vector2 GetNewRandomDirection()
    {
        // Tính toán vector từ trung tâm đến vị trí hiện tại của nhân vật
        Vector2 currentPosition = transform.position;
        Vector2 directionToCenter = (Vector2.zero - currentPosition).normalized;

        // Tính toán góc từ hướng hiện tại đến trung tâm
        float angleToCenter = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;

        // Tạo một góc ngẫu nhiên trong khoảng 90 độ về phía bên ngoài (không quay lại trung tâm)
        float randomAngle = Random.Range(-searchAngle, searchAngle);
        float newAngle = angleToCenter + 180 + randomAngle;

        // Chuyển đổi góc độ thành radian
        float radians = newAngle * Mathf.Deg2Rad;

        // Tính toán vector hướng mới
        Vector2 newDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        return newDirection;
    }

    private Vector2 GetRandomPositionInDirection(Vector2 direction)
    {
        // Tạo một góc ngẫu nhiên trong khoảng -45 đến 45 độ phía trước mặt
        float randomAngle = Random.Range(-searchAngle, searchAngle);

        // Chuyển đổi góc độ thành radian
        float radians = randomAngle * Mathf.Deg2Rad;

        // Tạo ma trận quay để xoay vector hướng
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        Vector2 rotatedDirection = new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );

        // Tạo vị trí ngẫu nhiên bằng cách thêm vector hướng đã quay vào vị trí hiện tại
        Vector2 randomPosition = (Vector2)transform.position + rotatedDirection * moveRadius;

        return randomPosition;
    }
}
