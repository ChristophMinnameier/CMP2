using UnityEngine;
using System.Collections;

public class Chessboard : MonoBehaviour {
	//--
	public int m_iSize = 10;

	GameObject[,] m_Grid;

	int GetAliveN(int _iCol, int _iRow)
	{
		int iAliveNeighbours = 0;
		// nachbarn für feld 3,8 im radius 1
		for (int iCol = _iCol-1; iCol <= _iCol; iCol++) {
			for (int iRow = _iRow-1; iRow <= _iRow; iRow++) {
				if (iCol == _iCol && iRow == _iRow) // don't check yourself
					continue;

				if (iCol >= 0 && iCol < m_iSize && iRow >= 0 && iRow < m_iSize && // check bounds
				    m_Grid[iCol, iRow].GetComponent<Renderer>().material.color == Color.black)
					iAliveNeighbours++;
			}
		}
		// alternativ für "don't check yourself":
		//if (m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color == Color.black)
		//	iAliveNeighbours--;

		return iAliveNeighbours;
	}

	void KillAll()
	{
		for (int i = 0; i < m_iSize; i++)
			for (int j = 0; j < m_iSize; j++)
				SetAlive (i, j, false);
	}


	// Use this for initialization
	void Start () {

		m_Grid = new GameObject[m_iSize,m_iSize];

		for (int i = 0; i < m_iSize; i++)
			for (int j = 0; j < m_iSize; j++) {
				GameObject kachel = GameObject.CreatePrimitive(PrimitiveType.Quad);
				m_Grid[i,j] = kachel;
			kachel.name = "kachel(" + i + "," + j + ")";
				kachel.transform.position = new Vector3(i, j, 0);
				kachel.transform.parent = this.transform;
				if (Random.value >= 0.5)
					SetAlive(i,j,true);

			}
		Camera.main.transform.position = new Vector3(m_iSize/2, m_iSize/2, -10);
		Camera.main.orthographicSize = m_iSize;
		
		transform.position = new Vector3 (0.5f, 0.5f, 0);

		print ("Anzahl Nachbarn: " + GetAliveN (1, 1));
	}

	// Update is called once per frame
	void Update () {

		
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int iIndexX = (int)mouseWorldPos.x;
			int iIndexY = (int)mouseWorldPos.y;
			if (iIndexX >= 0 && iIndexX < m_iSize && iIndexY >= 0 && iIndexY < m_iSize)
				Toggle(iIndexX, iIndexY);
		}

		if (Input.GetKeyDown(KeyCode.K))
			KillAll();

		if (Input.GetKeyDown(KeyCode.Space) == false)
			return;

		int[,] anzahlLebNach = new int[m_iSize, m_iSize];

		
		for (int iCol = 0; iCol < m_iSize; iCol++)
			for (int iRow = 0; iRow < m_iSize; iRow++)
				anzahlLebNach[iCol, iRow] = GetAliveNeighbours(iCol, iRow);

		for (int iCol = 0; iCol < m_iSize; iCol++)
			for (int iRow = 0; iRow < m_iSize; iRow++)
		{
			int iNumAlive = anzahlLebNach[iCol, iRow];
			// tile alive
			if (GetAlive(iCol, iRow))
			{
				if (iNumAlive < 2 || iNumAlive > 3)
					SetAlive(iCol, iRow, false);
					
			}
			// tile dead
			else
			{
				if (iNumAlive == 3)
					SetAlive(iCol, iRow, true);
			}
		}
	}

	void Toggle(int _iCol, int _iRow)
	{
		bool bAlive = GetAlive (_iCol, _iRow);
		SetAlive (_iCol, _iRow, !bAlive);
	}

    bool GetAlive(int _iCol, int _iRow)
    {
		return (m_Grid[_iCol, _iRow].GetComponent<Renderer>().material.color == Color.black);
	}
	/*
	void SetAlive(int _iCol, int _iRow)
	{
		m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color = Color.black;
	}
	void SetDead(int _iCol, int _iRow)
	{
		m_Grid [_iCol, _iRow].GetComponent<Renderer> ().material.color = Color.white;
	}*/

	void SetAlive(int _iCol, int _iRow, bool _bAlive)
    {
			if (_bAlive)
				m_Grid[_iCol, _iRow].GetComponent<Renderer>().material.color = Color.black;
			else
				m_Grid[_iCol, _iRow].GetComponent<Renderer>().material.color = Color.white;
	}

	int GetAliveNeighbours(int _iColumn,int _iRow)
	{
		int AliveNeighbours = 0;
		
		// check left neighbour
		int iLeftCol = _iColumn - 1;

		if (iLeftCol >= 0 && m_Grid[iLeftCol, _iRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}

		// check right  neighbour
		int iRightCol = _iColumn + 1;
		
		if (iRightCol < m_iSize && m_Grid[iRightCol, _iRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		// check upper neighbour
		int iUpRow = _iRow + 1;
		if (iUpRow < m_iSize && m_Grid[ _iColumn,iUpRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		// check lower neighbour
		int iDownRow = _iRow - 1;
		if (iDownRow >= 0 && m_Grid[ _iColumn,iDownRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		// check upper left neighbour
		if(iLeftCol >= 0 && iUpRow < m_iSize && m_Grid[iLeftCol, iUpRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		// check down left neighbour
		if(iLeftCol >= 0 && iDownRow >= 0 && m_Grid[iLeftCol, iDownRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		// check upper right neighbour
		if(iRightCol < m_iSize&& iUpRow < m_iSize && m_Grid[iRightCol, iUpRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		// check down right neighbour
		if(iRightCol < m_iSize && iDownRow >= 0 && m_Grid[iRightCol, iDownRow].GetComponent<Renderer> ().material.color == Color.black){
			AliveNeighbours ++;}
		
		return AliveNeighbours;
	}

}
