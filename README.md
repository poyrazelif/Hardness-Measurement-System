# Brinell-Vickers-Hardness-Measurement-System

Bu uygulama,cisimlerin sertlik değerlerinin bulunması amacı ile; önceden oluşturulmuş brinell ve vickers izlerinin mikroskop tarafından görüntülerinin alınması ve görüntü işleme teknikleri ile çap/köşegen uzunluklarının hassas tespiti için geliştirilmiştir.

1.	Kalibrasyon
2.	Manuel Ölçüm
3.	Otomatik Ölçüm başlıklarını içerir.

KALİBRASYON

Lenslerin mikrometre ile kalibrasyonuna ait örnek görüntü aşağıda gösterilmektedir.

![resim](https://user-images.githubusercontent.com/75081591/150598359-3fdad6ab-ae28-4de5-b814-1941fd808a3f.png)

MANUEL ÖLÇÜM

Manuel ölçüm izin köşegen veya çap gibi ölçümlerinin başlangıç ve bitiş noktalarının kullanıcı tarafından belirlenmesidir. Kullanıcı mavi ve yeşil renkteki iki işaretçi yardımı ile noktaları belirlemektedir. Şekilde bir vickers izinin manuel ölçümüne ait görsel gösterilmektedir.

![resim](https://user-images.githubusercontent.com/75081591/150598913-3d750a82-0058-48bc-a7cc-6e20da1a67a0.png)

OTOMATİK ÖLÇÜM

Otomatik ölçüm yönteminde kullanıcı tarafından, boyutları “Ayarlar” bölümünden ayarlanabilen bir dikdörtgenin istenilen köşe veya radyusa yerleştirilmesi beklenir. Ölçüm başlatıldığında program bu dikdörtgen içerisinde gerekli algoritmaları izleyerek izin vickers/brinell izi olma durumuna göre köşe veya yayın en dış noktasının tespitini yapmaktadır. Bu aşamada izin vickers/brinell izi olma durumuna ve dikdörtgenin izin sağ,sol,üst,alt tarafına yerleştrilme durumuna göre birçok farklı algoritma izlenmiştir. İlgili görüntüler aşağıda gösterilmektedir.

![resim](https://user-images.githubusercontent.com/75081591/150600482-9086bc4f-d479-4e13-86b2-5f52c9126944.png)
                                 Bir Vickers izine ait otomatik ölçüm
                                 
![resim](https://user-images.githubusercontent.com/75081591/150601059-b26b0009-4e43-4b9c-9b13-55f970bedd8f.png)
                                 Bir Brinell izine ait otomatik ölçüm
                                 
![resim](https://user-images.githubusercontent.com/75081591/150600769-a6302e29-f915-4398-af5f-11aef91c57a6.png)
                                 Gürültülü vickers izinde köşe noktalarının tespiti
                                 

EK BİLGİLER

Otomatik ölçüm sırasında bahsedilen eşik değeri kavramı, kontrastı çok düşük veya çok yüksek görüntülerin de ölçümünün yapılabilmesi için kullanıcı tarafından belirli bir skala boyunca değiştirilebilir yapılmıştır. Programın “Ayarlar” kısmındaki eşik değerinin değiştirilmesi ile yumuşak geçişli görüntülerin de ölçümü yapılabilmektedir.

Kameranın yazılım ile bağlantısı için IC imaging control 3.5 .NET kütühanesi kullanılmıştır.



