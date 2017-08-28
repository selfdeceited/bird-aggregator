import * as React from "react"
import UnderConstructionState from './UnderConstructionState'
import axios from 'axios';
import ReactMapboxGl, { Layer, Feature, ZoomControl, Popup } from "react-mapbox-gl"
import { Link } from 'react-router-dom'
import {BirdPopup} from './BirdPopup'

const Map = ReactMapboxGl({
    accessToken: "pk.eyJ1IjoidG9ueXJ5emhpa292IiwiYSI6ImNpbHhvYTY0MDA4MTF0bWtyaW9xbjAyaWsifQ.ih-8rDMRiBmDPqdeyyrHNg"
});

// todo: fix https://github.com/mapbox/mapbox-gl-js/issues/2440
// enhance according to: http://alex3165.github.io/react-mapbox-gl/demos
// add clusterisation

interface MapWrapProps {}
interface MapWrapState {
    markers: MapMarkerDto[],
    selectedMarker: MapMarkerDto,
    zoomLevel: number[],
    center: number[]
}
interface MapMarkerDto {
    id: number,
    x: number,
    y: number,
    birds: BirdDto[]
}

export interface BirdDto{
    id: number,
    name: string
}

export class MapWrap extends React.Component<MapWrapProps, MapWrapState> {
    constructor(props) {
        super(props);
        this.state = {
            markers: [],
            selectedMarker: undefined,
            zoomLevel: [6],
            center: [35.5, 55.6]
        };
    }

    componentDidMount() {
        axios.get(`/api/birds/map/markers`).then(res => {
            const markers = res.data as MapMarkerDto[];
            this.setState({ markers });
        });
    }

    markerClick(selectedMarker: MapMarkerDto, { feature }: { feature: any }){
        this.setState({ selectedMarker: selectedMarker,
            center: feature.geometry.coordinates,
            zoomLevel: this.state.zoomLevel, });
    }
    removePopup(){
        this.setState({ selectedMarker: undefined });
    }
    render() {
        return (
<div className="body">
<Map
  style="mapbox://styles/mapbox/outdoors-v10"
  containerStyle={{
    height: "100vh",
    width: "100vw"
  }}
  center={this.state.center}
  zoom={this.state.zoomLevel}
  >
    <Layer
      type="symbol"
      id="marker"
      layout={{ "icon-image": "circle-15" }}
      >

      { this.state.markers.map(x => (
        <Feature
            key= {x.id}
            coordinates={[x.x, x.y]} properties={x}
            onClick={this.markerClick.bind(this, x)}
        />
    ))}
    </Layer>
    {
            this.state.selectedMarker && (
              <Popup
                key={this.state.selectedMarker.id}
                offset={[0, -50]}
                coordinates={[this.state.selectedMarker.x, this.state.selectedMarker.y]}
              >
                  <div className="map-popup">
                        <a className="pt-button pt-minimal small-reference pt-icon-cross" onClick={() => this.removePopup()}></a>
                        <BirdPopup birds={this.state.selectedMarker.birds}></BirdPopup>
                  </div>
              </Popup>
            )
          }
    <ZoomControl/>
</Map>
</div>);
    }
}