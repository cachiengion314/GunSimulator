using System;
using Firebase.Analytics;
using HoangNam;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPSystem : MonoBehaviour, IDetailedStoreListener
{
  public static IAPSystem Instance { get; private set; }

  // Tạo một cấu trúc để nhập dữ liệu từ Inspector
  [Serializable]
  public struct IAPProduct
  {
    public string productId;
    public ProductType productType; // Consumable, NonConsumable, Subscription
  }

  // Danh sách các sản phẩm IAP nhập từ Inspector
  public IAPProduct[] products;

  private IStoreController storeController;
  private IExtensionProvider extensionProvider;

  // Callback trả về kết quả mua hàng
  private Action<bool, string> purchaseCallback;
  private bool isRestoreSuccse = false;

  private void Awake()
  {
    // Đảm bảo đây là Singleton
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    InitializePurchasing();
  }

  public void InitializePurchasing()
  {
    if (IsInitialized())
    {
      return;
    }

    var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

    // Tự động thêm các sản phẩm được khai báo từ Inspector vào cấu hình IAP
    foreach (var product in products)
    {
      builder.AddProduct(product.productId, product.productType);
    }

    UnityPurchasing.Initialize(this, builder); // Sử dụng IDetailedStoreListener
  }

  private bool IsInitialized()
  {
    return storeController != null && extensionProvider != null;
  }

  public string GetLocalizedPrice(int productIndex)
  {
    if (Debug.isDebugBuild) return "";
    string productId = products[productIndex].productId;

    if (IsInitialized())
    {
      // Lấy sản phẩm sau khi khởi tạo thành công
      Product product = storeController.products.WithID(productId);

      if (product != null)
      {
        // Lấy giá của sản phẩm
        string localizedPrice = product.metadata.localizedPriceString;
        return localizedPrice;
      }
      else
      {
        Debug.LogWarning("Không tìm thấy sản phẩm hoặc sản phẩm chưa có giá.");
        return null;
      }
    }
    else
    {
      Debug.LogWarning("Unity IAP chưa được khởi tạo. Không thể lấy giá.");
      return null;
    }
  }

  // Hàm mua sản phẩm, truyền vào productId và callback trả về bool
  public void PurchaseProduct(int productIndex, Action<bool, string> callback)
  {
    string productId = products[productIndex].productId;

    ShowLoading();
    if (IsInitialized())
    {
      if (FirebaseSetup.Instance.IsFirebaseReady)
      {
        FirebaseAnalytics.LogEvent(HoangNam.KeyStr.FIREBASE_INAPP_EVENT);
        FirebaseAnalytics.LogEvent(HoangNam.KeyStr.FIREBASE_INAPP_PROCESS);
      }

      Product product = storeController.products.WithID(productId);

      if (product != null && product.availableToPurchase)
      {
        Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");
        purchaseCallback = callback; // Lưu callback
        storeController.InitiatePurchase(product);
      }
      else
      {
        Debug.Log("BuyProductID: FAIL. Product not found or not available for purchase");
        callback?.Invoke(false, productId);
        HideLoding();
      }
    }
    else
    {
      Debug.Log("BuyProductID FAIL. Not initialized.");
      callback?.Invoke(false, productId);
      HideLoding();
    }
  }

  // Hàm khôi phục giao dịch
  public void RestorePurchases(Action<bool, string> callback)
  {
    ShowLoading();
    if (!IsInitialized())
    {
      Debug.LogWarning("Unity IAP chưa được khởi tạo. Không thể khôi phục giao dịch.");
      HideLoding();
      return;
    }

    purchaseCallback = callback;
    isRestoreSuccse = false;

    // Phân biệt nền tảng đang chạy
#if UNITY_EDITOR
    Debug.Log("Đang chạy trên Unity Editor. Không có khôi phục giao dịch thực tế.");
    RestorePurchasesEditor();
#elif UNITY_IOS
        Debug.Log("Đang khôi phục giao dịch trên iOS...");
        extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, message) =>
        {
            if (result)
            {
                // Các sản phẩm sẽ được xử lý trong ProcessPurchase
                if(!isRestoreSuccse)
                {
                  callback?.Invoke(false, message);
                  HideLoding();
                  Debug.LogWarning($"Không có giao dịch nào để khôi phục trên iOS. Thông báo: {message}");
                }
            }
            else
            {
              callback?.Invoke(false, message);
              HideLoding();
              Debug.LogWarning($"Không thể khôi phục trên iOS. Thông báo: {message}");
            }
        });
#elif UNITY_ANDROID
        Debug.Log("Đang khôi phục giao dịch trên Android...");
        extensionProvider.GetExtension<IGooglePlayStoreExtensions>().RestoreTransactions((result, message) =>
        {
            if (result)
            {
                // Các sản phẩm sẽ được xử lý trong ProcessPurchase
                if(!isRestoreSuccse)
                {
                  callback?.Invoke(false, message);
                  HideLoding();
                  Debug.LogWarning($"Không có giao dịch nào để khôi phục trên Android. Thông báo: {message}");
                }
            }
            else
            {
              callback?.Invoke(false, message);
              HideLoding();
              Debug.LogWarning($"Không thể khôi phục trên Android. Thông báo: {message}");
            }
        });
#else
        Debug.LogWarning("Nền tảng này không hỗ trợ khôi phục giao dịch.");
#endif
  }

  // Xử lý khi khởi tạo IAP thành công
  public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
  {
    storeController = controller;
    extensionProvider = extensions;
    Debug.Log("Unity IAP đã khởi tạo thành công.");
  }

  // Xử lý khi khởi tạo IAP thất bại
  public void OnInitializeFailed(InitializationFailureReason error)
  {
    Debug.LogError("Khởi tạo IAP thất bại: " + error);
  }
  public void OnInitializeFailed(InitializationFailureReason error, string message)
  {
    var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

    if (message != null)
    {
      errorMessage += $" More details: {message}";
    }

    Debug.Log(errorMessage);
  }

  // Xử lý giao dịch mua thành công và khôi phục sản phẩm
  public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
  {
    if (args.purchasedProduct.hasReceipt)
    {
      Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");

      // Trả về kết quả thành công thông qua callback nếu có
      purchaseCallback?.Invoke(true, args.purchasedProduct.definition.id);
      isRestoreSuccse = true;

      if (FirebaseSetup.Instance.IsFirebaseReady)
      {
        FirebaseAnalytics.LogEvent(HoangNam.KeyStr.FIREBASE_INAPP_SUCCESS);
      }
      HideLoding();
      return PurchaseProcessingResult.Complete;
    }
    return PurchaseProcessingResult.Pending;
  }

  // Xử lý khi mua hàng thất bại
  public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
  {
    Debug.LogError($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
    purchaseCallback?.Invoke(false, product.definition.storeSpecificId);
    HideLoding();

    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(HoangNam.KeyStr.FIREBASE_INAPP_FAIL);
    }
  }

  public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
  {
    purchaseCallback?.Invoke(false, product.definition.storeSpecificId);
    HideLoding();

    if (FirebaseSetup.Instance.IsFirebaseReady)
    {
      FirebaseAnalytics.LogEvent(HoangNam.KeyStr.FIREBASE_INAPP_FAIL);
    }
    Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
  }

  void ShowLoading()
  {
    UIManager.Instance.GetUI<BaseUIRoot>(KeyStr.NAME_LOADING).Show();
  }
  void HideLoding()
  {
    UIManager.Instance.GetUI<BaseUIRoot>(KeyStr.NAME_LOADING).Hide();
  }

  private void RestorePurchasesEditor()
  {
    HideLoding();
  }
}