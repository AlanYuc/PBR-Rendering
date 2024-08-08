using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterControl : MonoBehaviour
{
    private GameObject currentModedl;
    private GameObject directionalLight;
    private Light dirLight;
    private Material currentMaterial;

    public GameObject rusted_Iron;
    public GameObject rfa;
    public GameObject container;
    public GameObject pot;
    public GameObject plaid;
    private Transform rusted_IronTrans;
    private Transform rfaTrans; 
    private Transform containerTrans;
    private Transform potTrans;
    private Transform plaidTrans;

    private bool isSliderChange = false;

    //RFA part panel
    private Transform rfaPart;
    private bool isRfaOn = false;
    private Toggle rfaHair;
    private Toggle rfaCloth;

    private enum Properties
    {
        Roughness,
        Metallic,
        Gloss,
        BumpScale,
        LightColor,
        LightIntensity
    }

    //Roughness
    private Text roughnessText;
    private Slider roughnessSlider;

    //Metallic
    private Text metallicText;
    private Slider metallicSlider;
    private Toggle metallicToggle;

    //Gloss
    private Text glossText;
    private Slider glossSlider;

    //BumpScale
    private Text bumpText;
    private Slider bumpSlider;

    //GammaCorrect
    private Text gammaCorrectText;
    private Slider gammaCorrectSlider;

    //SpecularIntensity
    private Text specularIntensityText;
    private Slider specularIntensitySlider;

    //LightColor
    private Text lightColorRedText;
    private Slider lightColorRedSlider;
    private Text lightColorGreenText;
    private Slider lightColorGreenSlider;
    private Text lightColorBlueText;
    private Slider lightColorBlueSlider;

    //LightIntensity
    private Text lightIntensityText;
    private Slider lightIntensitySlider;

    //BRDF Toggle
    private Toggle cookTorranceToggle;
    private Toggle kajiyaKayToggle;
    private Toggle ashikhminShirleyToggle;
    private Toggle christensenBurleyToggle;
    private Toggle orenNayarToggle;
    private Toggle pbrDisneyToggle;

    private float useCookTorrance = 1;
    private float usekajiyaKay = 0;
    private float useAshikhminShirley = 0;
    private float useChristensenBurley = 0;
    private float useOrenNayar = 0;
    private float usePbrDisney = 0;
    private float defalutCookTorrance = 1;
    private float defalutkajiyaKay = 0;
    private float defalutAshikhminShirley = 0;
    private float defalutChristensenBurley = 0;
    private float defalutOrenNayar = 0;
    private float defalutPbrDisney = 0;

    //Set Default Value
    private float roughnessDefault = 0.5f;
    private float metallicDefault = 0.5f;
    private float glossDefault = 20;
    private float bumpDefault = 1;
    private float gammaDefault = 2.2f;
    private float specularIDefault = 1;
    private float lightColorRedDefault = 1;
    private float lightColorGreenDefault = 1;
    private float lightColorBlueDefault = 1;
    private float lightIntensityDefault = 1;


    // Start is called before the first frame update
    void Start()
    {
        rusted_IronTrans = rusted_Iron.GetComponent<Transform>();
        rfaTrans = rfa.GetComponent<Transform>();
        containerTrans = container.GetComponent<Transform>();
        potTrans = pot.GetComponent<Transform>();
        plaidTrans = plaid.GetComponent<Transform>();

        rfaPart = transform.Find("../ModelPanel/RFA_Part");
        if (rfaPart == null)
        {
            Debug.Log("no rfa");
        }
        rfaHair = rfaPart.transform.Find("Hair").GetComponent<Toggle>();
        rfaCloth = rfaPart.transform.Find("Cloth").GetComponent<Toggle>();
        rfaPart.gameObject.SetActive(false);

        directionalLight = GameObject.FindWithTag("Directional Light");
        dirLight = directionalLight.GetComponent<Light>();
        //Debug.Log(dirLight);

        //GetComponent
        roughnessText           = transform.Find("ShaderProperties/Roughness/Value").GetComponent<Text>();
        roughnessSlider         = transform.Find("ShaderProperties/Roughness/Slider").GetComponent<Slider>();

        metallicText            = transform.Find("ShaderProperties/Metallic/Value").GetComponent<Text>();
        metallicSlider          = transform.Find("ShaderProperties/Metallic/Slider").GetComponent<Slider>();
        metallicToggle          = transform.Find("ShaderProperties/Metallic/UseMap").GetComponent<Toggle>();

        glossText               = transform.Find("ShaderProperties/Gloss/Value").GetComponent<Text>();
        glossSlider             = transform.Find("ShaderProperties/Gloss/Slider").GetComponent<Slider>();

        bumpText                = transform.Find("ShaderProperties/BumpScale/Value").GetComponent<Text>();
        bumpSlider              = transform.Find("ShaderProperties/BumpScale/Slider").GetComponent<Slider>();

        gammaCorrectText        = transform.Find("ShaderProperties/GammaCorrect/Value").GetComponent<Text>();
        gammaCorrectSlider      = transform.Find("ShaderProperties/GammaCorrect/Slider").GetComponent<Slider>();

        specularIntensityText   = transform.Find("ShaderProperties/SpecularIntensity/Value").GetComponent<Text>();
        specularIntensitySlider = transform.Find("ShaderProperties/SpecularIntensity/Slider").GetComponent<Slider>();

        lightColorRedText       = transform.Find("LightSettings/LightColor/ValueR").GetComponent<Text>();
        lightColorRedSlider     = transform.Find("LightSettings/LightColor/SliderR").GetComponent<Slider>();

        lightColorGreenText     = transform.Find("LightSettings/LightColor/ValueG").GetComponent<Text>();
        lightColorGreenSlider   = transform.Find("LightSettings/LightColor/SliderG").GetComponent<Slider>();

        lightColorBlueText      = transform.Find("LightSettings/LightColor/ValueB").GetComponent<Text>();
        lightColorBlueSlider    = transform.Find("LightSettings/LightColor/SliderB").GetComponent<Slider>();

        lightIntensityText      = transform.Find("LightSettings/Intensity/Value").GetComponent<Text>();
        lightIntensitySlider    = transform.Find("LightSettings/Intensity/Slider").GetComponent<Slider>();

        cookTorranceToggle      = transform.Find("BRDF/Cook-Torrance").GetComponent<Toggle>();
        kajiyaKayToggle         = transform.Find("BRDF/Kajiya-Kay").GetComponent<Toggle>();
        ashikhminShirleyToggle  = transform.Find("BRDF/Ashikhmin-Shirley").GetComponent<Toggle>();
        christensenBurleyToggle = transform.Find("BRDF/Christensen-Burley").GetComponent<Toggle>();
        orenNayarToggle         = transform.Find("BRDF/Oren-Nayar").GetComponent<Toggle>();
        pbrDisneyToggle         = transform.Find("BRDF/PBR Disney").GetComponent<Toggle>();


        //测试，暂时使用场景中默认的模型，之后要动态获取
        //currentModedl = GameObject.FindWithTag("Player");
        currentModedl = rusted_Iron;
        rfaTrans.gameObject.SetActive(false);
        potTrans.gameObject.SetActive(false);
        plaidTrans.gameObject.SetActive(false);
        containerTrans.gameObject.SetActive(false);

        if(currentModedl!= null)
        {
            Debug.Log(currentModedl);
        }
        else
        {
            Debug.Log("no find");
        }

        //有的模型是MeshRenderer，有的是SkinnedMeshRenderer，需要分开
        currentMaterial = currentModedl.GetComponent<MeshRenderer>().material;

        //Default Setting
        roughnessText.text = currentMaterial.GetFloat("_Roughness").ToString();
        roughnessSlider.value = currentMaterial.GetFloat("_Roughness");

        metallicText.text = currentMaterial.GetFloat("_Metallic").ToString();
        metallicSlider.value = currentMaterial.GetFloat("_Metallic");

        //Gloss - (2,64)
        glossText.text = currentMaterial.GetFloat("_Gloss").ToString();
        glossSlider.minValue = 2;
        glossSlider.maxValue = 64;
        glossSlider.value = currentMaterial.GetFloat("_Gloss");

        //BumpScale - (-100 , 100)
        bumpText.text = currentMaterial.GetFloat("_BumpScale").ToString();
        bumpSlider.minValue = -100;
        bumpSlider.maxValue = 100;
        bumpSlider.value = currentMaterial.GetFloat("_BumpScale");

        //GammaCorrect -(1 , 5)
        gammaCorrectText.text = currentMaterial.GetFloat("_GammaCorrect").ToString();
        gammaCorrectSlider.minValue = 1;
        gammaCorrectSlider.maxValue = 5;
        gammaCorrectSlider.value = currentMaterial.GetFloat("_GammaCorrect");

        //SpecularIntensity - (1 , 100)
        specularIntensityText.text = currentMaterial.GetFloat("_SpecularIntensity").ToString();
        specularIntensitySlider.minValue = 1;
        specularIntensitySlider.maxValue = 100;
        specularIntensitySlider.value = currentMaterial.GetFloat("_SpecularIntensity");

        //Debug.Log(dirLight.color.r); (0,1)
        lightColorRedText.text = dirLight.color.r.ToString();
        lightColorRedSlider.value = dirLight.color.r;

        lightColorGreenText.text = dirLight.color.g.ToString();
        lightColorGreenSlider.value = dirLight.color.g;

        lightColorBlueText.text = dirLight.color.b.ToString();
        lightColorBlueSlider.value = dirLight.color.b;

        //Intensity - (0,200)
        lightIntensityText.text = dirLight.intensity.ToString();
        lightIntensitySlider.maxValue = 200;
        lightIntensitySlider.value = dirLight.intensity;

        currentMaterial.SetFloat("_UseMetallicMap", 0);
        SetDefaultValue();
        SetBrdfDefault();
    }

    // Update is called once per frame
    void Update()
    {
        //string.Format("")
        float roughness         = Mathf.Round(roughnessSlider.value * 1000f) / 1000f;
        float metallic          = Mathf.Round(metallicSlider.value * 1000f) / 1000f;
        float gloss             = Mathf.Round(glossSlider.value * 1000f) / 1000f;
        float bump              = Mathf.Round(bumpSlider.value * 1000f) / 1000f;
        float gamma             = Mathf.Round(gammaCorrectSlider.value * 1000f) / 1000f;
        float specularI         = Mathf.Round(specularIntensitySlider.value * 1000f) / 1000f;
        float lightColorRed     = Mathf.Round(lightColorRedSlider.value * 1000f) / 1000f;
        float lightColorGreen   = Mathf.Round(lightColorGreenSlider.value * 1000f) / 1000f;
        float lightColorBlue    = Mathf.Round(lightColorBlueSlider.value * 1000f) / 1000f;
        float lightIntensity    = Mathf.Round(lightIntensitySlider.value * 1000f) / 1000f;

        roughnessText.text          = roughness.ToString();
        metallicText.text           = metallic.ToString();
        glossText.text              = gloss.ToString();
        bumpText.text               = bump.ToString();
        gammaCorrectText.text       = gamma.ToString();
        specularIntensityText.text  = specularI.ToString();
        lightColorRedText.text      = lightColorRed.ToString();
        lightColorGreenText.text    = lightColorGreen.ToString();
        lightColorBlueText.text     = lightColorBlue.ToString();
        lightIntensityText.text     = lightIntensity.ToString();

        currentMaterial.SetFloat("_Roughness", roughness);
        currentMaterial.SetFloat("_Metallic", metallic);
        currentMaterial.SetFloat("_Gloss", gloss);
        currentMaterial.SetFloat("_BumpScale", bump);
        currentMaterial.SetFloat("_GammaCorrect", gamma);
        currentMaterial.SetFloat("_SpecularIntensity", specularI);

        if(currentModedl.name == "RFA_Model")
        {
            Material clothMat = currentModedl.transform.Find("geo/Cloth").GetComponent<SkinnedMeshRenderer>().material;
            clothMat.SetFloat("_Roughness", roughness);
            clothMat.SetFloat("_Metallic", metallic);
            clothMat.SetFloat("_Gloss", gloss);
            clothMat.SetFloat("_BumpScale", bump);
            clothMat.SetFloat("_GammaCorrect", gamma);
            clothMat.SetFloat("_SpecularIntensity", specularI);
        }

        Color newColor = new Color(lightColorRed, lightColorGreen, lightColorBlue);
        dirLight.color = newColor;
        dirLight.intensity = lightIntensity;

    }

    private void SetDefaultValue()
    {
        roughnessText.text          = roughnessDefault.ToString();
        metallicText.text           = metallicDefault.ToString();
        glossText.text              = glossDefault.ToString();
        bumpText.text               = bumpDefault.ToString();
        gammaCorrectText.text       = gammaDefault.ToString();
        specularIntensityText.text  = specularIDefault.ToString();
        lightColorRedText.text      = lightColorRedDefault.ToString();
        lightColorGreenText.text    = lightColorGreenDefault.ToString();
        lightColorBlueText.text     = lightColorBlueDefault.ToString();
        lightIntensityText.text     = lightIntensityDefault.ToString();

        roughnessSlider.value           = roughnessDefault;
        metallicSlider.value            = metallicDefault;
        glossSlider.value               = glossDefault;
        bumpSlider.value                = bumpDefault;
        gammaCorrectSlider.value        = gammaDefault;
        specularIntensitySlider.value   = specularIDefault;
        lightColorRedSlider.value       = lightColorRedDefault;
        lightColorBlueSlider.value      = lightColorBlueDefault;
        lightColorGreenSlider.value     = lightColorGreenDefault;
        lightIntensitySlider.value      = lightIntensityDefault;

        metallicToggle.isOn = false;
    }

    private void SetBrdfDefault()
    {
        currentMaterial.SetFloat("_UseCook_Torrance", defalutCookTorrance);
        currentMaterial.SetFloat("_UseKajiya_Kay", defalutkajiyaKay);
        currentMaterial.SetFloat("_UseAshikhmin_Shirley", defalutAshikhminShirley);
        currentMaterial.SetFloat("_UseChristensen_Burley", defalutChristensenBurley);
        currentMaterial.SetFloat("_UseOren_Nayar", defalutOrenNayar);
        currentMaterial.SetFloat("_UsePBR_Disney", defalutPbrDisney);

        useCookTorrance = defalutCookTorrance;
        usekajiyaKay = defalutkajiyaKay;
        useAshikhminShirley = defalutAshikhminShirley;
        useChristensenBurley = defalutChristensenBurley;
        useOrenNayar = defalutOrenNayar;
        usePbrDisney = defalutPbrDisney;

        cookTorranceToggle.isOn = true;
    }

    private void SetBRDF()
    {
        currentMaterial.SetFloat("_UseCook_Torrance", useCookTorrance);
        currentMaterial.SetFloat("_UseKajiya_Kay", usekajiyaKay);
        currentMaterial.SetFloat("_UseAshikhmin_Shirley", useAshikhminShirley);
        currentMaterial.SetFloat("_UseChristensen_Burley", useChristensenBurley);
        currentMaterial.SetFloat("_UseOren_Nayar", useOrenNayar);
        currentMaterial.SetFloat("_UsePBR_Disney", usePbrDisney);
    }

    public void OnRustedIronChanged(bool isOn)
    {
        if (isOn)
        {
            rusted_IronTrans.gameObject.SetActive(true);
            rfaTrans.gameObject.SetActive(false);
            containerTrans.gameObject.SetActive(false);
            potTrans.gameObject.SetActive(false);
            plaidTrans.gameObject.SetActive(false);

            currentModedl = rusted_IronTrans.gameObject;
            currentMaterial = currentModedl.GetComponent<MeshRenderer>().material;
            SetDefaultValue();
            SetBrdfDefault();
        }
    }

    public void OnRfaChanged(bool isOn)
    {
        if (isOn)
        {
            rfaPart.gameObject.SetActive(true);
            isRfaOn = true;

            rfaTrans.gameObject.SetActive(true);
            rusted_IronTrans.gameObject.SetActive(false);
            containerTrans.gameObject.SetActive(false);
            potTrans.gameObject.SetActive(false);
            plaidTrans.gameObject.SetActive(false);


            currentModedl = rfaTrans.gameObject;


            //hair default
            currentMaterial = currentModedl.transform.Find("geo/Hair").GetComponent<SkinnedMeshRenderer>().material;
            SetDefaultValue();
            SetBrdfDefault();

            rfaHair.isOn = true;

            Debug.Log(currentModedl.name);
        }
        else
        {
            rfaPart.gameObject.SetActive(false);
            isRfaOn = false;
        }
    }

    public void OnPotChanged(bool isOn)
    {
        if (isOn)
        {
            rusted_IronTrans.gameObject.SetActive(false);
            rfaTrans.gameObject.SetActive(false);
            containerTrans.gameObject.SetActive(false);
            potTrans.gameObject.SetActive(true);
            plaidTrans.gameObject.SetActive(false);

            currentModedl = potTrans.gameObject;
            currentMaterial = currentModedl.GetComponentInChildren<MeshRenderer>().material;

            SetDefaultValue();
            SetBrdfDefault();
        }
    }

    public void OnContainerChanged(bool isOn)
    {
        rusted_IronTrans.gameObject.SetActive(false);
        rfaTrans.gameObject.SetActive(false);
        containerTrans.gameObject.SetActive(true);
        potTrans.gameObject.SetActive(false);
        plaidTrans.gameObject.SetActive(false);

        currentModedl = containerTrans.gameObject;
        currentMaterial = currentModedl.GetComponent<MeshRenderer>().material;

        SetDefaultValue();
        SetBrdfDefault();
    }

    public void OnPlaidChanged(bool isOn)
    {
        rusted_IronTrans.gameObject.SetActive(false);
        rfaTrans.gameObject.SetActive(false);
        containerTrans.gameObject.SetActive(false);
        potTrans.gameObject.SetActive(false);
        plaidTrans.gameObject.SetActive(true);

        currentModedl = plaidTrans.gameObject;
        currentMaterial = currentModedl.GetComponent<MeshRenderer>().material;

        SetDefaultValue();
        SetBrdfDefault();
    }

    public void OnRoughnessResetClick()
    {
        roughnessText.text = roughnessDefault.ToString();
        roughnessSlider.value = roughnessDefault;
    }
    public void OnMetallicResetClick()
    {
        metallicText.text = metallicDefault.ToString();
        metallicSlider.value = metallicDefault;
    }

    public void OnGlossResetClick()
    {
        glossText.text = glossDefault.ToString();
        glossSlider.value = glossDefault;
    }
    public void OnBumpScaleResetClick()
    {
        bumpText.text = bumpDefault.ToString();
        bumpSlider.value = bumpDefault;
    }
    public void OnGammaCorrectResetClick()
    {
        gammaCorrectText.text = gammaDefault.ToString();
        gammaCorrectSlider.value = gammaDefault;
    }

    public void OnSpecularIntensityResetClick()
    {
        specularIntensityText.text = specularIDefault.ToString();
        specularIntensitySlider.value = specularIDefault;
    }

    public void OnLightSettingsResetClick()
    {
        lightColorRedText.text = lightColorRedDefault.ToString();
        lightColorGreenText.text = lightColorGreenDefault.ToString();
        lightColorBlueText.text = lightColorBlueDefault.ToString();
        lightColorRedSlider.value = lightColorRedDefault;
        lightColorBlueSlider.value = lightColorBlueDefault;
        lightColorGreenSlider.value = lightColorGreenDefault;
    }

    public void OnLightIntensityResetClick()
    {
        lightIntensityText.text = lightIntensityDefault.ToString();
        lightIntensitySlider.value = lightIntensityDefault;
    }

    public void OnUseMetallicMapToggleChanged(bool isOn)
    {
        if (isOn)
        {
            //Use MetallicMap
            currentMaterial.SetFloat("_UseMetallicMap", 1);
        }
        else
        {
            currentMaterial.SetFloat("_UseMetallicMap", 0);
        }
    }

    public void OnUseCookTorrance(bool isOn)
    {
        if (isOn)
        {
            useCookTorrance = 1;
            usekajiyaKay = 0;
            useAshikhminShirley = 0;
            useChristensenBurley = 0;
            useOrenNayar = 0;
            usePbrDisney = 0;
            SetBRDF();
        }
    }

    public void OnUsekajiyaKay(bool isOn)
    {
        if (isOn)
        {
            useCookTorrance = 0;
            usekajiyaKay = 1;
            useAshikhminShirley = 0;
            useChristensenBurley = 0;
            useOrenNayar = 0;
            usePbrDisney = 0;
            SetBRDF();
        }
    }

    public void OnUseAshikhminShirley(bool isOn)
    {
        if (isOn)
        {
            useCookTorrance = 0;
            usekajiyaKay = 0;
            useAshikhminShirley = 1;
            useChristensenBurley = 0;
            useOrenNayar = 0;
            usePbrDisney = 0;
            SetBRDF();
        }
    }

    public void OnUseChristensenBurley(bool isOn)
    {
        if (isOn)
        {
            useCookTorrance = 0;
            usekajiyaKay = 0;
            useAshikhminShirley = 0;
            useChristensenBurley = 1;
            useOrenNayar = 0;
            usePbrDisney = 0;
            SetBRDF();
        }
    }

    public void OnUseOrenNayar(bool isOn)
    {
        if (isOn)
        {
            useCookTorrance = 0;
            usekajiyaKay = 0;
            useAshikhminShirley = 0;
            useChristensenBurley = 0;
            useOrenNayar = 1;
            usePbrDisney = 0;
            SetBRDF();
        }
    }

    public void OnUsePbrDisney(bool isOn)
    {
        if (isOn)
        {
            useCookTorrance = 0;
            usekajiyaKay = 0;
            useAshikhminShirley = 0;
            useChristensenBurley = 0;
            useOrenNayar = 0;
            usePbrDisney = 1;
            SetBRDF();
        }
    }

    public void OnRfaHairSelected(bool isOn)
    {
        if (isOn)
        {
            currentMaterial = currentModedl.transform.Find("geo/Hair").GetComponent<SkinnedMeshRenderer>().material;

            SetDefaultValue();
            SetBrdfDefault();
        }
    }

    public void OnRfaClothSelected(bool isOn)
    {
        if (isOn)
        {
            currentMaterial = currentModedl.transform.Find("geo/Cloth").GetComponent<SkinnedMeshRenderer>().material;

            SetDefaultValue();
            SetBrdfDefault();
        }
    }
}
