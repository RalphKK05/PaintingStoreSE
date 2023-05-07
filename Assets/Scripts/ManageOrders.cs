using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageOrders : MonoBehaviour
{

    public Dropdown dropdown;
    private string selectedOption;
    public Text StatusText;
    public GameObject StatusBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowStatusBox(){
        StatusBox.SetActive(true);
    }
    public void ChangeStatus(){
        selectedOption = dropdown.options[dropdown.value].text;
        StatusText.text="Status: "+selectedOption;
        StatusBox.SetActive(false);
        GameObject adminorderPage = GameObject.FindWithTag("AdminOrderPage");
        adminorderPage.GetComponent<AdminOrder>().UpdateStatus(GetComponent<OrderItem>().orderID, selectedOption);

    }
    public void DeleteOrder()
    {
        GameObject adminorderPage = GameObject.FindWithTag("AdminOrderPage");
        adminorderPage.GetComponent<AdminOrder>().DeleteOrder(GetComponent<OrderItem>().orderID);
    
    }
}
