using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject TAB_BAR;
    public GameObject FEED_PAGE;
    public GameObject PAINTING_PAGE;
    public GameObject CART_PAGE;
    public GameObject SEARCH_PAGE;
    public GameObject ACCOUNT_PAGE;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseAllPages()
    {
        FEED_PAGE.SetActive(false);
        CART_PAGE.SetActive(false);
        SEARCH_PAGE.SetActive(false);
        PAINTING_PAGE.SetActive(false);
        ACCOUNT_PAGE.SetActive(false);
    }
    public void OpenFEED_PAGE()
    {
        CloseAllPages();
        FEED_PAGE.SetActive(true);
        FEED_PAGE.GetComponent<FeedPage>().LoadFeed();
        TAB_BAR.SetActive(true);
    }

    public void OpenCART_PAGE()
    {
        CloseAllPages();
        CART_PAGE.SetActive(true);
        TAB_BAR.SetActive(true);
        CART_PAGE.GetComponent<CartPage>().LoadCart();
    }
    public void OpenSEARCH_PAGE()
    {
        CloseAllPages();
        SEARCH_PAGE.SetActive(true);
        TAB_BAR.SetActive(true);
        SEARCH_PAGE.GetComponent<SearchPage>().OpenSearch();
    }

    public void OpenACCOUNT_PAGE()
    {
        CloseAllPages();
        ACCOUNT_PAGE.SetActive(true);
        TAB_BAR.SetActive(true);
        ACCOUNT_PAGE.GetComponent<AccountPage>().LoadStore();
    }
    public GameObject OpenPAINTING_PAGE()
    {
        GameObject newPaintingPage = Instantiate(PAINTING_PAGE, PAINTING_PAGE.transform.position, PAINTING_PAGE.transform.rotation);
        newPaintingPage.transform.parent = canvas.transform;
        newPaintingPage.SetActive(true);
        return newPaintingPage;
    }
}
