using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;



public class SearchPage : MonoBehaviour
{
    bool clickOnce;
    public GameObject NoResult;
    public GameObject FilterPage, SortPage;
    public Toggle priceLowHighToggle, priceHighLowToggle, creationNewOldToggle, creationOldNewToggle, uploadNewOldToggle, uploadOldNewToggle, ratingLowHighToggle, ratingHighLowToggle, noneToggle;
    public GameObject filterList;
    public RectTransform filterListRectTransform;
    public Vector3 endClosePosition, endOpenPosition;
    public float filterListMoveSpeed;
    public Queries queries;
    public GameObject paintingDisplay;
    public GameObject feedscrollParent;
    public List<GameObject> StyleToggles = new List<GameObject>();
    public List<GameObject> UploadDateToggles = new List<GameObject>();
    public List<GameObject> CreationDateToggles = new List<GameObject>();
    public InputField titleInput, minPriceInput, maxPriceInput, artistInput, minHInput, minWInput, maxHInput, maxWInput;
    public GameObject star1, star2, star3, star4, star5;
    public GameObject star1Full, star2Full, star3Full, star4Full, star5Full;
    List<GameObject> searchresultsList = new List<GameObject>();
    List<string> styleFilter = new List<string>();
    public string title;
    public string minPrice, maxPrice;
    public string minRating;
    public string artistFName, artistLName;
    public string uploadDate, creationDate;
    public string preferences;
    public string minH, minW, maxH, maxW;
    public string orderParameter;
    public string artistParameter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void SubmitInput()
    {
        CheckFilters();
        CheckOrders();
        ExecuteSearch();
    }
    public void OpenSearch()
    {
        ResetSearch();
    }
    public void ResetSearch()
    {
        CloseFilters();
        CloseSort();
        titleInput.text = "";
        minPriceInput.text = "";
        maxPriceInput.text = "";
        artistInput.text = "";
        minHInput.text = "";
        maxHInput.text = "";
        minWInput.text = "";
        maxWInput.text = "";
        noneToggle.isOn = true;
        CreationDateToggles[0].GetComponent<Toggle>().isOn = true;
        UploadDateToggles[0].GetComponent<Toggle>().isOn = true;

    }
    public void OpenFilters()
    {
        FilterPage.SetActive(true);

    }
    public void Star1()
    {
        clickOnce = false;
        if (star1Full.activeInHierarchy)
        {
            clickOnce = true;
            star1Full.SetActive(false);
            star2Full.SetActive(false);
            star3Full.SetActive(false);
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star1Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
        }


    }

    public void Star2()
    {
        clickOnce = false;
        if (star2Full.activeInHierarchy)
        {
            clickOnce = true;
            star2Full.SetActive(false);
            star3Full.SetActive(false);
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star2Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
        }


    }

    public void Star3()
    {
        clickOnce = false;
        if (star3Full.activeInHierarchy)
        {
            clickOnce = true;
            star3Full.SetActive(false);
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star3Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
            star3Full.SetActive(true);
        }


    }

    public void Star4()
    {
        clickOnce = false;
        if (star4Full.activeInHierarchy)
        {
            clickOnce = true;
            star4Full.SetActive(false);
            star5Full.SetActive(false);

        }
        if (star4Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
            star3Full.SetActive(true);
            star4Full.SetActive(true);
        }


    }

    public void Star5()
    {
        clickOnce = false;
        if (star5Full.activeInHierarchy)
        {
            clickOnce = true;
            star5Full.SetActive(false);

        }
        if (star5Full.activeInHierarchy == false && clickOnce == false)
        {
            star1Full.SetActive(true);
            star2Full.SetActive(true);
            star3Full.SetActive(true);
            star4Full.SetActive(true);
            star5Full.SetActive(true);
        }


    }

    public void CheckFilters()
    {
        if (titleInput.text == "")
        {
            title = "'%'";
        }
        if (titleInput.text != "")
        {
            title = "'%" + titleInput.text + "%'";
        }

        styleFilter.Clear();
        foreach (GameObject prefToggle in StyleToggles)
        {
            if (prefToggle.GetComponent<Toggle>().isOn)
            {
                styleFilter.Add(prefToggle.GetComponent<PreferenceToggleLabel>().PreferenceLabel);
            }
        }

        if (styleFilter.Count != 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(");

            for (int i = 0; i < styleFilter.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append("'" + styleFilter[i] + "'");
            }

            builder.Append(")");

            preferences = "IN " + builder.ToString();
        }
        else
        {
            preferences = "LIKE '%'";
        }


        if (minPriceInput.text == "" && maxPriceInput.text != "")
        {
            minPrice = "0";
            maxPrice = maxPriceInput.text;
        }
        if (minPriceInput.text == "" && maxPriceInput.text == "")
        {
            minPrice = "0";
            maxPrice = "9999999999";
        }
        if (minPriceInput.text != "" && maxPriceInput.text == "")
        {
            minPrice = "0";
            maxPrice = "9999999999";
        }
        if (minPriceInput.text != "" && maxPriceInput.text != "")
        {
            minPrice = minPriceInput.text;
            maxPrice = maxPriceInput.text;
        }


        if (star1Full.activeInHierarchy == false)
        {
            minRating = "0";
        }

        if (star1Full.activeInHierarchy && star2Full.activeInHierarchy == false)
        {
            minRating = "1";
        }
        if (star2Full.activeInHierarchy && star3Full.activeInHierarchy == false)
        {
            minRating = "2";
        }
        if (star3Full.activeInHierarchy && star4Full.activeInHierarchy == false)
        {
            minRating = "3";
        }
        if (star4Full.activeInHierarchy && star5Full.activeInHierarchy == false)
        {
            minRating = "4";
        }
        if (star5Full.activeInHierarchy)
        {
            minRating = "5";
        }


        if (artistInput.text.Contains(" "))
        {
            string[] parts = artistInput.text.Split(' ');
            artistFName = parts[0];
            artistLName = parts[1];
        }
        else
        {
            artistFName = artistInput.text;
            artistLName = artistInput.text;
        }

        foreach (GameObject toggle in UploadDateToggles)
        {
            if (toggle.GetComponent<Toggle>().isOn)
            {
                if (toggle.GetComponent<PreferenceToggleLabel>().PreferenceLabel == "9999 YEAR")
                {
                    uploadDate = "";
                }
                else
                {
                    uploadDate = " AND uploadDate >= DATE_SUB(NOW(), INTERVAL " + toggle.GetComponent<PreferenceToggleLabel>().PreferenceLabel + ")";
                }
               
            }
        }

        foreach (GameObject toggle in CreationDateToggles)
        {
            if (toggle.GetComponent<Toggle>().isOn)
            {
                if (toggle.GetComponent<PreferenceToggleLabel>().PreferenceLabel == "9999 YEAR")
                {
                    creationDate = "";
                }
                else
                {
                    creationDate = " AND creationDate >= DATE_SUB(NOW(), INTERVAL " + toggle.GetComponent<PreferenceToggleLabel>().PreferenceLabel + ")";
                }
            }
        }

        minH = minHInput.text;
        minW = minWInput.text;
        maxH = maxHInput.text;
        maxW = maxWInput.text;

        if (artistInput.text.Contains(" "))
        {
            artistParameter = "AND (artist_fName = '" + artistFName + "' AND artist_lName = '" + artistLName + "')";
        }
        else
        {
            artistParameter = "AND (artist_fName = '" + artistFName + "' OR artist_lName = '" + artistLName + "')";
        }
        if (artistInput.text == "")
        {
            artistParameter = "";
        }
    }
    public void ConfirmFilterChanges()
    {
        CheckFilters();
        CheckOrders();
        CloseFilters();
        ExecuteSearch();



        //ADD SIZE FILTERS
        
        //LINK THE SORT AND FILTER BUTTONS AND SEARCH BAR, IF NTG IN FILTER, RESET WHATS IN FILTER AND SORT W HEK.
    }

    public void ExecuteSearch()
    {
        foreach (GameObject result in searchresultsList)
        {
            Destroy(result);
        }
        searchresultsList.Clear();
        CallGetQueryResultSEARCH(queries.WriteQuery_SEARCH(title, minPrice, maxPrice, minRating, uploadDate, preferences, creationDate, artistParameter, orderParameter));

    }

    public void CheckOrders()
    {
        if (noneToggle.isOn)
        {
            orderParameter = "";
        }



        if (priceLowHighToggle.isOn)
        {
            orderParameter = "ORDER BY price ASC";
        }
        if (priceHighLowToggle.isOn)
        {
            orderParameter = "ORDER BY price DESC";
        }

        if (creationNewOldToggle.isOn)
        {
            orderParameter = "ORDER BY creationDate DESC";
        }
        if (creationOldNewToggle.isOn)
        {
            orderParameter = "ORDER BY creationDate ASC";
        }

        if (uploadNewOldToggle.isOn)
        {
            orderParameter = "ORDER BY uploadDate DESC";
        }
        if (uploadOldNewToggle.isOn)
        {
            orderParameter = "ORDER BY uploadDate ASC";
        }

        if (ratingLowHighToggle.isOn)
        {
            orderParameter = "ORDER BY rating ASC";
        }
        if (ratingHighLowToggle.isOn)
        {
            orderParameter = "ORDER BY rating DESC";
        }

    }
    public void ConfirmOrderChanges()
    {
        CheckOrders();
        CheckFilters();
        CloseSort();
        ExecuteSearch();
    }

    public void CloseFilters()
    {
        FilterPage.SetActive(false);
    }

    public void CloseSort()
    {
        SortPage.SetActive(false);
    }

    public void OpenSort()
    {
        SortPage.SetActive(true);
    }

    public void CallGetQueryResultSEARCH(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            if (result == "0 results")
            {
                NoResult.SetActive(true);
            }
            else
            {
                NoResult.SetActive(false);
                DisplayPaintings(queries.ExtractInfo(result));
            }
           

        }));
    }



    void DisplayPaintings(List<List<string>> feedpaintings)
    {

        for (int i = 0; i < feedpaintings.Count; i++)
        {


            GameObject painting = Instantiate(paintingDisplay, transform.position, transform.rotation);
            painting.transform.parent = feedscrollParent.transform;
            painting.GetComponent<SetPaintingDisplayInfo>().pID = feedpaintings[i][0];
            painting.GetComponent<SetPaintingDisplayInfo>().title = feedpaintings[i][1];
            painting.GetComponent<SetPaintingDisplayInfo>().price = feedpaintings[i][2];
            painting.GetComponent<SetPaintingDisplayInfo>().imageURL = feedpaintings[i][3];
            painting.GetComponent<SetPaintingDisplayInfo>().description = feedpaintings[i][4];
            painting.GetComponent<SetPaintingDisplayInfo>().rating = feedpaintings[i][5];
            painting.GetComponent<SetPaintingDisplayInfo>().size = feedpaintings[i][6];
            painting.GetComponent<SetPaintingDisplayInfo>().style = feedpaintings[i][7];
            painting.GetComponent<SetPaintingDisplayInfo>().artistFName = feedpaintings[i][8];
            painting.GetComponent<SetPaintingDisplayInfo>().artistLName = feedpaintings[i][9];
            painting.GetComponent<SetPaintingDisplayInfo>().qt = feedpaintings[i][12];
            painting.GetComponent<SetPaintingDisplayInfo>().artistID = feedpaintings[i][13];
            searchresultsList.Add(painting);
         



        }

    }

    public void MoveToPosition(Vector3 targetPosition, float moveTime)
    {
        // Set the end position of the object to the target position


        // Set the starting position of the object to its current position
        float startY = filterListRectTransform.anchoredPosition.y;
        print(startY);
        float targetY = -34;
        // Start the MoveCoroutine
        StartCoroutine(MovePageCoroutine(startY, targetY, moveTime));
    }

    private IEnumerator MovePageCoroutine(float startY, float targetY, float moveTime)
    {
        // Set isMoving to true

        
        float dist = Mathf.Abs(targetY - startY);
        float incr = dist / moveTime;


        // Reset the elapsed time to 0
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            // Calculate the interpolation value (0 to 1)
            //float t = elapsedTime * moveTime;

            // Move the object using linear interpolation
            filterListRectTransform.anchoredPosition += new Vector2(0f, incr) * Time.deltaTime;

            // Increase the elapsed time since the last frame
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Set the object to the end position once the movement is complete
        //filterListRectTransform.position = targetPosition;

        // Set isMoving to false

    }
    private IEnumerator MoveCoroutine(Vector3 startPosition, Vector3 targetPosition, float moveTime)
    {
        // Set isMoving to true
   

        // Reset the elapsed time to 0
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            // Calculate the interpolation value (0 to 1)
            //float t = elapsedTime * moveTime;

            // Move the object using linear interpolation
            filterListRectTransform.anchoredPosition += new Vector2(0f, 5700) * Time.deltaTime;

            // Increase the elapsed time since the last frame
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Set the object to the end position once the movement is complete
        //filterListRectTransform.position = targetPosition;

        // Set isMoving to false
     
    }
}
