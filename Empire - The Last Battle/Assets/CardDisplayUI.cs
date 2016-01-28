﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class CardDisplayUI : MonoBehaviour 
{
    public delegate void CardAction(CardData cardData);
    public event CardAction OnCardUse = delegate { };

    public HandUI _HandUI;
    public Image _LeftCard;
    public Image _RightCard;
    public Image _CentreCard;
    public Color _LandR_Colour;
    CanvasGroup _canvasGroup;
    int _currentCentreIndex;

    bool _isShowing;
    public bool _IsShowing
    {
        get { return _isShowing; }
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //put a wheel scroll thing????? Maybe.
	}

    public void Show()
    {
        //make visible 
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        _isShowing = true;
    }

    public void Hide()
    {
        //make invisible
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
        _isShowing = false;

        //deselect
        _HandUI.DeselectCurrent();
    }

    public void Init()
    {
        //grab canvas group
        _canvasGroup = this.GetComponent<CanvasGroup>();

        //events
        _HandUI.OnCardSelect += _HandUI_OnCardSelect;
        _HandUI.OnHandSet += _HandUI_OnHandSet;
        _HandUI.OnCardDeselect += _HandUI_OnCardDeselect;

        //hide
        Hide();

        //------TEST-------
        if(_HandUI.m_Cards.Count > 0)
            _HandUI.SelectCard(0);
    }

    void _HandUI_OnCardDeselect()
    {
        Debug.Log("Deselect");
        Hide();
    }

    void _HandUI_OnHandSet()
    {
        //if there is a card selected then use its index, if not then use index 0
        int index = (_HandUI.m_SelectedCardUI != null) ? _HandUI.m_SelectedCardUI._Index : 0;
        int r_Index = (index == _HandUI.m_Cards.Count - 1) ? -1 : index + 1;
        int l_Index = (index == 0) ? -1 : index - 1;

        //focused sprite
        _CentreCard.sprite = _HandUI.GetSpriteOfCard(_HandUI.m_Cards[index]._Card.Type);

        //Hide the sideimages if they dont represent a card.
        if (r_Index != -1)
        {
            _RightCard.sprite = _HandUI.GetSpriteOfCard(_HandUI.m_Cards[r_Index]._Card.Type);
            _RightCard.color = _LandR_Colour;
        }
        else
            _RightCard.color = new Color(0, 0, 0, 0);

        if (l_Index != -1)
        {
            _LeftCard.sprite = _HandUI.GetSpriteOfCard(_HandUI.m_Cards[l_Index]._Card.Type);
            _LeftCard.color = _LandR_Colour;
        }
        else
            _LeftCard.color = new Color(0, 0, 0, 0);

        //remenber selected index
        _currentCentreIndex = index;
    }

    void _HandUI_OnCardSelect()
    {
        //swich the card sprites so that selected is front.
        _HandUI_OnHandSet();

        //show
        if (!_isShowing)
            Show();
    }

    public void UseSelectedCardHandler()
    {
        //event!
        OnCardUse(_HandUI.m_SelectedCardUI._Card);
    }

    public void OnScrollLeft()
    {
        //select next card left
        if (_currentCentreIndex > 0)
            _HandUI.SelectCard(_currentCentreIndex - 1);
    }

    public void OnScrollRight()
    {
        //select next card right
        if (_currentCentreIndex < _HandUI.m_Cards.Count - 1)
            _HandUI.SelectCard(_currentCentreIndex + 1);
    }

    public void OnBackClickedHandler()
    {
        //hide 
        Hide();
    }

}
