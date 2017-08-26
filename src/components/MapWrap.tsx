import * as React from "react"
import UnderConstructionState from './UnderConstructionState'
import axios from 'axios';
import ReactMapboxGl, { Layer, Feature, ZoomControl, Popup } from "react-mapbox-gl"

const Map = ReactMapboxGl({
    accessToken: "pk.eyJ1IjoidG9ueXJ5emhpa292IiwiYSI6ImNpbHhvYTY0MDA4MTF0bWtyaW9xbjAyaWsifQ.ih-8rDMRiBmDPqdeyyrHNg"
});

// todo: fix https://github.com/mapbox/mapbox-gl-js/issues/2440
// enhance according to: http://alex3165.github.io/react-mapbox-gl/demos

interface MapWrapProps {}
interface MapWrapState {
    markers: MapMarkerDto[]
}
interface MapMarkerDto {
    id: number,
    x: number,
    y: number,
    birdNames: string
}

export class MapWrap extends React.Component<MapWrapProps, MapWrapState> {
    constructor(props) {
        super(props);
        this.state = {
            markers: []
        };
    }

    componentDidMount() {
        axios.get(`/api/birds/map/markers`).then(res => {
            const markers = res.data as MapMarkerDto[];
            this.setState({ markers });
        });
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
  center={[35.5, 55.6]}
  zoom={[6]}
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
        />
    ))}
    </Layer>
    <ZoomControl/>
</Map>
</div>);
    }
}